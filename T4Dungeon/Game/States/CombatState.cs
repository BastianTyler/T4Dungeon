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

    private CombatOrchestrator _combat;
    private UIContext _ui;

    public CombatState(
        StateMachine fsm,
        InputSystem input,
        GameLogSystem log,
        MapManager map,
        Player player)
    {
        _fsm = fsm;
        _input = input;
        _log = log;
        _map = map;
        _player = player;
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
        SetMenu();
    }

    public void Update()
    {
        if (_ui == null) return;

        ConsoleRenderer.Render(
            _map,
            _ui,
            _log.Active.ToList(),
            null,
            false,
            true,
            _combat?.Enemy
        );

        int choice = _input.GetSelection(_ui.Options.Count);
        _ui.Options[choice].Action?.Invoke();
    }

    private void SetMenu()
    {
        _ui = MenuFactory.CreateCombatMenu(
            onAttack: () =>
            {
                _combat.PlayerAttack();
            },

            onSkill: () => SetSkillMenu(),

            onDefend: () =>
            {
                _combat.PlayerDefend();
            },

            onFlee: () =>
            {
                if (_combat.TryFlee())
                    _fsm.ChangeState(GameStateType.Exploration);
                else
                    _combat.ResolveAfterPlayer();
            },

            onInv: () => { }
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
                SetMenu();
            },
            onBack: () => SetMenu()
        );
    }

    private void ForceRender()
    {
        if (_combat == null) return;

        ConsoleRenderer.Render(
            _map,
            _ui,
            _log.Active.ToList(),
            null,
            false,
            true,
            _combat.Enemy
        );
    }
}