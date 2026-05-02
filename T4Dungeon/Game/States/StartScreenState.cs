using T4Dungeon.Game.Core;
using T4Dungeon.Game.Models;
using T4Dungeon.Game.States;
using T4Dungeon.Game.Utils;

public class StartScreenState : IGameState
{
    private readonly StateMachine _fsm;
    private readonly InputSystem _input;

    private UIContext _ui;

    public StartScreenState(StateMachine fsm, InputSystem input)
    {
        _fsm = fsm;
        _input = input;
    }

    public void Enter()
    {
        _ui = MenuFactory.CreateStartMenu(
            onAdventure: () => _fsm.ChangeState(GameStateType.Intro),
            onClassic: () => _fsm.ChangeState(GameStateType.Exploration),
            onTutorial: () => _fsm.ChangeState(GameStateType.Tutorial),
            onExit: () => Environment.Exit(0)
        );
    }

    public void Update()
    {
        ConsoleRenderer.Render(null, _ui, new List<string>(), null, false, false);

        int choice = _input.GetSelection(_ui.Options.Count);

        var option = _ui.Options[choice];

        if (!option.IsImplemented)
            return;

        option.Action?.Invoke();
    }

    public void Exit() { }
}