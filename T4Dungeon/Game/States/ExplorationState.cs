using T4Dungeon.Game.Core;
using T4Dungeon.Game.Models;
using T4Dungeon.Game.States;
using T4Dungeon.Game.Systems;
using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

public class ExplorationState : IGameState
{
    private readonly StateMachine _fsm;
    private readonly InputSystem _input;
    private readonly MapManager _map;
    private readonly Player _player;
    private readonly GameLogSystem _log;
    private readonly CombatManager _combatManager;

    private bool _transitioning;
    private bool _initialized = false;

    private UIContext _ui;

    public ExplorationState(
        StateMachine fsm,
        InputSystem input,
        MapManager map,
        Player player,
        GameLogSystem log,
        CombatManager combatManager)
    {
        _fsm = fsm;
        _input = input;
        _map = map;
        _player = player;
        _log = log;
        _combatManager = combatManager;
    }

    // =========================
    // LIFECYCLE
    // =========================

    public void Enter()
    {
        _transitioning = false;
        if (!_initialized)
        {
            _map.GenerateMap();
            _player.Inventory.Add(ItemId.HealthPotion, 1);
            _player.Inventory.Add(ItemId.IronSword, 1);
            _initialized = true;
        }
        BuildMenu();
    }

    public void Update()
    {
        ConsoleRenderer.Render(_map, _ui, _log.Active.ToList(), _player, false, false);

        int choice = _input.GetSelection(_ui.Options.Count);

        HandleChoice(choice);
    }

    public void Exit() { }

    // =========================
    // INPUT
    // =========================

    private void HandleChoice(int choice)
    {
        _ui.Options[choice].Action?.Invoke();
    }

    // =========================
    // MENU
    // =========================

    private void BuildMenu()
    {
        _ui = MenuFactory.CreateMainMenu(
            onMove: MoveMode,
            onEquip: OpenEquipment,
            onInv: OpenInventory,
            onExit: ExitGame
        );
    }

    // =========================
    // MOVEMENT
    // =========================

    private void MovePlayer(int dx, int dy)
    {
        if (!_map.TryMove(dx, dy))
        {
            _log.Add("You can't move outside the map.");
            return;
        }
        _log.Debug($"Moved to {_map.PlayerPosition.X}-{_map.PlayerPosition.Y}");

        HandleCell();
    }

    private void MoveMode()
    {
        _ui = MenuFactory.CreateMoveMenu(
                 up: () => MovePlayer(0, -1),
                 down: () => MovePlayer(0, 1),
                 left: () => MovePlayer(-1, 0),
                 right: () => MovePlayer(1, 0),
                 back: ReturnToMainMenu
             );
    }

    // =========================
    // CELL INTERACTION
    // =========================

    private void HandleCell()
    {
        if (_transitioning)
            return;

        var cell = _map.GetCurrentCell();
        var cellEvent = CellEventFactory.Create(cell.Type);

        string result = cellEvent.Execute(_player, cell);

        if (!string.IsNullOrEmpty(result))
        {
            bool isCombat = result == "Combat";
            //_log.Add(result, !isCombat); // ❗ no pause if entering combat
        }

        if (result == "Combat")
        {
            _transitioning = true;
            var enemy = _combatManager.CreateRandomEnemy();

            _log.Add($"{TextColor.Yellow}A {enemy.Name} appears!{TextColor.Reset}");
            Console.Clear();
            ConsoleRenderer.Render(_map, _ui, _log.Active.ToList(), null, false, false, enemy);
            Console.WriteLine("\n -- Press any key -- ");
            Console.ReadKey(true);
            while (Console.KeyAvailable) Console.ReadKey(true);

            var combat = new CombatOrchestrator(
                player: _player,
                enemy: enemy,
                rules: new CombatRulesSystem(),
                enemyAI: new EnemyActionSystem(),
                minigames: new MinigameSystem(),
                loot: new LootSystem(_log),
                log: _log
            );

            var combatState = (CombatState)_fsm.GetState(GameStateType.Combat);
            combatState.StartCombat(combat);

            combat.OnVictory = () =>
            {
                _map.ClearCell(cell);
                _fsm.ChangeState(GameStateType.Exploration);
            };
            combat.OnDefeat = () => _fsm.ChangeState(GameStateType.Exploration);


            _fsm.ChangeState(GameStateType.Combat);
            return;
        }

        if (result == "Shop")
        {
            _transitioning = true;
            _log.Add($"{TextColor.Cyan}A traveling merchant awaits...{TextColor.Reset}");

            // 1. Get the state from the FSM
            var shopState = (ShopState)_fsm.GetState(GameStateType.Shop);

            // 2. Setup the shop logic and inventory[cite: 7]
            shopState.StartShop();

            // 3. Define what happens when the player leaves
            shopState.OnExit = () =>
            {
                _map.ClearCell(cell); // Clear the shop tile
                _fsm.ChangeState(GameStateType.Exploration); // Return to moving[cite: 5]
            };

            // 4. Switch to the ShopState loop[cite: 5]
            _fsm.ChangeState(GameStateType.Shop);
            return;
        }

        _map.ClearCell(cell);
    }


    // =========================
    // EQUIPTMENT
    // =========================
    private void OpenEquipment()
    {
        SetEquipmentMenu();
    }

    private void SetEquipmentMenu()
    {
        _ui = MenuFactory.CreateEquipmentMenu(
            _player,
            onSelectSlot: SetEquipmentSelectMenu,
            onBack: BuildMenu
        );
    }

    private void SetEquipmentSelectMenu(EquiptSlot slot)
    {
        _ui = MenuFactory.CreateItemSelectMenu(
            _player,
            slot,
            onEquip: (id) =>
            {
                EquipItem(slot, id);
                SetEquipmentMenu();
            },
            onBack: SetEquipmentMenu
        );
    }

    private void EquipItem(EquiptSlot slot, ItemId newItemId)
    {
        _player.Equipment[slot] = newItemId;

        var item = ItemDatabase.Items[newItemId];
        _log.Add($"Equipped {item.Name}");
    }

    // =========================
    // INVENTORY
    // =========================
    private void OpenInventory()
    {
        SetInventoryMenu();
    }

    private void SetInventoryMenu()
    {
        _ui = MenuFactory.CreateInventoryMenu(
            _player,
            onUse: UseItem,
            onBack: BuildMenu
        );
    }

    private void UseItem(ItemId id)
    {
        Enemy enemy = null; // exploration has no enemy context

        var result = InventorySystem.UseItem(
            _player,
            id,
            enemy,
            _map
        );

        if (!string.IsNullOrEmpty(result.Message))
            _log.Add(result.Message);

        if (result.NeedsMapRedraw)
        {
            // map changed (e.g. Illuminate)
            ConsoleRenderer.Render(_map, _ui, _log.Active.ToList(), _player, false, false);
        }

        SetInventoryMenu();
    }

    //==========================================================
    private void ReturnToMainMenu()
    {
        BuildMenu();
    }
    private void ExitGame()
    {
        _fsm.ChangeState(GameStateType.StartScreen);
    }
}