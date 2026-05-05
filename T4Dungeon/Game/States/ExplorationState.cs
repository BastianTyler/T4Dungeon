using T4Dungeon.Game.Core;
using T4Dungeon.Game.Events;
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
    private readonly NarrativeDirector _narrativeDirector;

    private bool _transitioning;
    private bool _initialized = false;

    private UIContext _ui;

    public ExplorationState(StateMachine fsm, InputSystem input, MapManager map, Player player, GameLogSystem log, CombatManager combatManager, NarrativeDirector narrativeDirector)
    {
        _fsm = fsm;
        _input = input;
        _map = map;
        _player = player;
        _log = log;
        _combatManager = combatManager;
        _narrativeDirector = narrativeDirector;

        _narrativeDirector.OnMapLoadRequested += HandleMapLoad;
        _narrativeDirector.OnNarrativeMessage += HandleNarrativeMessage;
        _narrativeDirector.OnStartingItemsRequested += HandleStartingItems;
        _narrativeDirector.OnStartingEquipmentRequested += HandleStartingEquipment;
    }

    // =========================
    // LIFECYCLE
    // =========================

    public void Enter()
    {
        _transitioning = false;
        _narrativeDirector.OnEvent("returned_to_exploration");

        if (!_initialized)
        {
            if (!_narrativeDirector.IsActive)
            {
                _map.GenerateMap();
                _player.Inventory.Add(ItemId.HealthPotion, 1);
                _player.Inventory.Add(ItemId.IronSword, 1);
                _player.Inventory.Add(ItemId.FireScroll, 1);
                _player.Inventory.Add(ItemId.StormPendant, 1);
            }
            _initialized = true;
        }

        BuildMenu();
    }

    public void Update()
    {
        //ConsoleRenderer.Render(_map, _ui, _log.Active.ToList(), _player, false, false);
        ConsoleRenderer.Render(GameStateType.Exploration, _ui, _log.Active.ToList(), _player, false, _map);

        int choice = _input.GetSelection(_ui.Options.Count);

        HandleChoice(choice);
    }

    public void Exit()
    {
    }

    // =========================
    // INPUT
    // =========================

    private void HandleChoice(int choice)
    {
        var option = _ui.Options[choice];

        if (_narrativeDirector.IsActive)
        {
            string yell = _narrativeDirector.ValidateChoice(option.Text);
            if (yell != null)
            {
                _log.Add(yell, waitForKey: true);
                return; // block the action, stay where they are
            }
        }

        option.Action?.Invoke();
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
        _narrativeDirector.OnEvent("move_menu_opened");
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

        if(cellEvent is EmptyEvent)
        {
            _log.Add($"{TextColor.White}Nothing here...{TextColor.Reset}");
        }

        if (cellEvent is TreasureEvent)
        {
            _log.Add(result);
            _map.ClearCell(cell);
            return;
        }

        if (cellEvent is CombatEvent)
        {
            _transitioning = true;
            var enemyOverride = _narrativeDirector.ConsumeEnemyOverride();
            var enemy = enemyOverride.HasValue
                ? _combatManager.CreateEnemy(enemyOverride.Value)
                : _combatManager.CreateRandomEnemy(_map.CurrentTier);

            _log.Add($"{TextColor.Yellow}A {enemy.Name} appears!{TextColor.Reset}");
            Console.Clear();
            //ConsoleRenderer.Render(_map, _ui, _log.Active.ToList(), null, false, false, enemy); // Old renderer logic
            ConsoleRenderer.Render(GameStateType.Exploration, _ui, _log.Active.ToList(), _player, false, _map, enemy);
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
                log: _log,
                narrativeDirector : _narrativeDirector
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

        if (cellEvent is ShopEvent)
        {
            _transitioning = true;
            _log.Add($"{TextColor.Cyan}A traveling merchant awaits...{TextColor.Reset}");

            var shopState = (ShopState)_fsm.GetState(GameStateType.Shop);

            shopState.StartShop();


            shopState.OnExit = () =>
            {
                _map.ClearCell(cell); 
                _fsm.ChangeState(GameStateType.Exploration); 
            };

            _fsm.ChangeState(GameStateType.Shop);
            return;
        }

        if (cellEvent is ExitEvent)
        {
            _transitioning = true;

            // Spawn boss for this tier
            var bossId = GetBossForTier(_map.CurrentTier);
            var boss = _combatManager.CreateEnemy(bossId);

            _log.Add($"{TextColor.Red}A powerful presence blocks the way... {boss.Name} appears!{TextColor.Reset}");
            Console.Clear();
            ConsoleRenderer.Render(GameStateType.Exploration, _ui, _log.Active.ToList(), _player, false, _map, boss);
            Console.WriteLine("\n -- Press any key -- ");
            Console.ReadKey(true);
            while (Console.KeyAvailable) Console.ReadKey(true);

            var combat = new CombatOrchestrator(
                player: _player,
                enemy: boss,
                rules: new CombatRulesSystem(),
                enemyAI: new EnemyActionSystem(),
                minigames: new MinigameSystem(),
                loot: new LootSystem(_log),
                log: _log,
                narrativeDirector: _narrativeDirector
            );

            var combatState = (CombatState)_fsm.GetState(GameStateType.Combat);
            combatState.StartCombat(combat);

            combat.OnVictory = () =>
            {
                _log.Add($"{TextColor.Green}You find a staircase leading deeper...{TextColor.Reset}", waitForKey: true);
                _map.AdvanceTier();
                _log.Add($"--- Entering Tier {_map.CurrentTier} ---");
                _fsm.ChangeState(GameStateType.Exploration);
            };

            combat.OnDefeat = () => _fsm.ChangeState(GameStateType.Exploration);

            _fsm.ChangeState(GameStateType.Combat);
            return;
        }

        _map.ClearCell(cell);
    }

    // =========================
    // NARRATIVE
    // =========================

    private void HandleMapLoad(string path) => _map.LoadMapFromFile(path);
    private void HandleNarrativeMessage(string msg, bool waitForKey) => _log.Add(msg, waitForKey);
    private void HandleStartingItems(ItemId[] items)
    {
        foreach (var id in items)
            _player.Inventory.Add(id, 1);
    }
    private void HandleStartingEquipment(EquiptSlot slot, ItemId id)
    {
        _player.Equipment[slot] = id;
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
        _narrativeDirector.OnEvent("inventory_opened");
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
        if (id == ItemId.Torch)
            _narrativeDirector.OnEvent("torch_used");

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
            //ConsoleRenderer.Render(_map, _ui, _log.Active.ToList(), _player, false, false);
            ConsoleRenderer.Render(GameStateType.Exploration,_ui,_log.Active.ToList(),_player,false,_map);
        }

        SetInventoryMenu();
    }

    private EnemyId GetBossForTier(int tier) => tier switch
    {
        1 => EnemyId.QueenSlimeMagenta,           
        2 => EnemyId.Orc,              
        3 => EnemyId.Orc,              
        4 => EnemyId.TheNamelessKnight,
        _ => EnemyId.Orc
    };

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