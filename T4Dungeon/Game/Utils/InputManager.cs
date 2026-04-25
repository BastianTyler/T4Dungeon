using System.Runtime.InteropServices;

public static class InputManager
{
    private static readonly bool IsBrowser = RuntimeInformation.IsOSPlatform(OSPlatform.Create("BROWSER"));
    public static Queue<ConsoleKeyInfo> WebInputBuffer = new();

    public static bool KeyAvailable()
    {
        if (IsBrowser) return WebInputBuffer.Count > 0;
        return Console.KeyAvailable;
    }

    public static ConsoleKeyInfo ReadKey()
    {
        if (IsBrowser)
        {
            if (WebInputBuffer.Count > 0) return WebInputBuffer.Dequeue();
            return default; // Return empty if nothing is there
        }
        return Console.ReadKey(true);
    }
}