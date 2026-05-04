using T4Dungeon.Game.Core;
using T4Dungeon.Game.States;
using T4Dungeon.Game.Utils;

public class CutsceneState : IGameState
{
    private readonly StateMachine _fsm;
    private List<CutsceneBeat> _beats;
    private Action _onComplete;

    public CutsceneState(StateMachine fsm)
    {
        _fsm = fsm;
    }

    public void Play(List<CutsceneBeat> beats, Action onComplete)
    {
        _beats = beats;
        _onComplete = onComplete;
    }

    public void Enter()
    {
        if (_beats == null) return;

        foreach (var beat in _beats)
        {
            Console.Clear();

            if (!string.IsNullOrEmpty(beat.AsciiArt))
                Console.WriteLine(beat.AsciiArt);

            if (!string.IsNullOrEmpty(beat.Text))
            {
                Console.WriteLine();
                Console.WriteLine($"  {beat.Text}");
                Console.WriteLine();
            }

            if (beat.WaitForKey)
            {
                Console.WriteLine($"  {TextColor.Gray}-- Press any key to continue --{TextColor.Reset}");
                Console.ReadKey(true);
                while (Console.KeyAvailable) Console.ReadKey(true);
            }
            else if (beat.DelayMs > 0)
            {
                Thread.Sleep(beat.DelayMs);
            }
        }

        _onComplete?.Invoke();
    }

    public void Update() { }
    public void Exit() { }

    public static string LoadArt(string path)
    {
        if (!File.Exists(path)) return string.Empty;
        return string.Join("\n", File.ReadAllLines(path));
    }
}

public class CutsceneBeat
{
    public string Text { get; set; }
    public bool WaitForKey { get; set; }
    public int DelayMs { get; set; }      // auto-advance after delay
    public string AsciiArt { get; set; }
    public string SoundFile { get; set; }
}

