using T4Dungeon.Game.Core;
using T4Dungeon.Game.Models;
using T4Dungeon.Game.States;
using T4Dungeon.Game.Systems;
using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

public class CombatState : IGameState
{
    private readonly StateMachine _fsm;
    private readonly InputSystem _input;
    private readonly GameLogSystem _log;
    private readonly MapManager _map;
    private readonly Player _player;
    private readonly NarrativeDirector _narrativeDirector;

    private CombatOrchestrator _combat;
    private UIContext _ui;

    public CombatState(StateMachine fsm, InputSystem input, GameLogSystem log, MapManager map, Player player, NarrativeDirector narrativeDirector)
    {
        _fsm = fsm;
        _input = input;
        _log = log;
        _map = map;
        _player = player;
        _narrativeDirector = narrativeDirector;
    }

    public void Enter()
    {
        _log.OnLogAdded += ForceRender;
        SetMenu();
    }

    public void Exit()
    {
        _log.OnLogAdded -= ForceRender;
    }


    public void StartCombat(CombatOrchestrator combat)
    {
        _combat = combat;
        _narrativeDirector.OnEvent("combat_started");
        SetMenu();
    }

    public void Update()
    {
        if (_ui == null) return;

        ConsoleRenderer.Render(GameStateType.Combat, _ui, _log.Active.ToList(), _player, false, null, _combat.Enemy);

        int choice = _input.GetSelection(_ui.Options.Count);
        HandleChoice(choice);
    }

    private void HandleChoice(int choice)
    {
        var option = _ui.Options[choice];

        if (_narrativeDirector.IsActive)
        {
            string yell = _narrativeDirector.ValidateChoice(option.Text);
            if (yell != null)
            {
                _log.Add(yell, waitForKey: true);
                return;
            }
        }

        option.Action?.Invoke();
    }

    private void SetMenu()
    {

        _ui = MenuFactory.CreateCombatMenu(
            onAttack: () => {_combat.PlayerAttack();},
            onSkill: () => SetSkillMenu(),
            onDefend: () => { _combat.PlayerDefend();},

            onFlee: () =>
            {
                if (_combat.TryFlee())
                    _fsm.ChangeState(GameStateType.Exploration);
                else
                    _combat.ResolveAfterPlayer();
            },

            onInv: () => OpenInventory()
        );
        
    }

    private void SetSkillMenu()
    {
        var equippedSkills = _player.Equipment.Values
            .Where(id => id.HasValue)
            .Select(id => ItemDatabase.Items[id.Value])
            .SelectMany(item => item.GrantedSkills)
            .Select(skillId => SkillDatabase.Skills[skillId])
            .ToList();

        _ui = MenuFactory.CreateSkillMenu(
            equippedSkills,
            onSkillSelect: (id) =>
            {
                _combat.UseSkill(id);
                if(_narrativeDirector.IsTutorial) SetSkillMenu(); 
                else SetMenu();
            },
            onBack: () => SetMenu()
        );
        _narrativeDirector.OnEvent("skill_menu_opened");
    }

    private void ForceRender()
    {
        if (_combat == null) return;

        //ConsoleRenderer.Render(_map, _ui, _log.Active.ToList(), null, false, true, _combat.Enemy);
        ConsoleRenderer.Render(GameStateType.Combat, _ui, _log.Active.ToList(), _player, false, null, _combat.Enemy);
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
            onBack: () => SetMenu()
        );
    }

    private void UseItem(ItemId id)
    {
        _combat.UseItem(id);
        SetMenu();
    }
}