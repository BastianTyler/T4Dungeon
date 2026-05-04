namespace T4Dungeon.Game.Models;

public class StoryContext
{
    public string ActiveChapter { get; set; }
    public Dictionary<string, bool> Flags { get; set; } = new();
    public Dictionary<string, int> Counters { get; set; } = new();

    public bool HasFlag(string flag) => Flags.TryGetValue(flag, out var v) && v;
    public void SetFlag(string flag, bool value = true) => Flags[flag] = value;
    public void Increment(string counter) => Counters[counter] = (Counters.TryGetValue(counter, out var v) ? v : 0) + 1;
}

