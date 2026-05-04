public class GameLogSystem
{
    private readonly List<string> _main = new();
    private readonly List<string> _debug = new();

    public IReadOnlyList<string> Active => _main;
    public IReadOnlyList<string> DebugHistory => _debug;

    public event Action? OnLogAdded;

    public void Add(string msg, bool waitForKey = false, int sleepMs = 0)
    {
        _main.Add(msg);
        _debug.Add($"[MAIN] {msg}");
        Trim(_main);

        OnLogAdded?.Invoke();

        if (sleepMs > 0)
            Thread.Sleep(sleepMs);

        if (waitForKey)
            Pause();
    }

    public void Debug(string msg)
    {
        _debug.Add($"[DEBUG] {msg}");
    }

    private void Trim(List<string> list)
    {
        if (list.Count > 10)
            list.RemoveAt(0);
    }

    private void Pause()
    {
        Console.WriteLine("\n -- Press any key to continue -- ");
        Console.ReadKey(true);
        while (Console.KeyAvailable) Console.ReadKey(true);
    }
}