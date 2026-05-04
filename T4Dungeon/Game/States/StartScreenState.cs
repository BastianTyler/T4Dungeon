using T4Dungeon.Game.Core;
using T4Dungeon.Game.Models;
using T4Dungeon.Game.Narrative;
using T4Dungeon.Game.States;
using T4Dungeon.Game.Systems;
using T4Dungeon.Game.Utils;

public class StartScreenState : IGameState
{
    private readonly StateMachine _fsm;
    private readonly InputSystem _input;
    private readonly NarrativeDirector _narrativeDirector;

    private UIContext _ui;

    public StartScreenState(StateMachine fsm, InputSystem input, NarrativeDirector narrativeDirector)
    {
        _fsm = fsm;
        _input = input;
        _narrativeDirector = narrativeDirector;
    }

    public void Enter()
    {
        _ui = MenuFactory.CreateStartMenu(
           onAdventure: () =>
           {
               _narrativeDirector.Load(new StoryScript());

               string artPath = @"E:\VisualStudio\2026Repos\T4Dungeon\T4Dungeon\Art\Ascii Art\TeeForTowerEntrance.txt";
               var cutscene = (CutsceneState)_fsm.GetState(GameStateType.Cutscene);
               cutscene.Play(
                   new List<CutsceneBeat>
                   {
                        new CutsceneBeat { AsciiArt = CutsceneState.LoadArt(artPath), Text = "The world is dark...", WaitForKey = true },
                        new CutsceneBeat { AsciiArt = CutsceneState.LoadArt(artPath), Text = "Your journey begins.", WaitForKey = true },
                   },
                   onComplete: () => _fsm.ChangeState(GameStateType.Exploration)
               );
               _fsm.ChangeState(GameStateType.Cutscene);
           },
            onClassic: () => _fsm.ChangeState(GameStateType.Exploration),
            onTutorial: () =>
            {
                _narrativeDirector.Load(new TutorialScript());
                _fsm.ChangeState(GameStateType.Exploration);
            },
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