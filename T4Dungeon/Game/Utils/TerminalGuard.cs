using System;
using System.Runtime.InteropServices;

namespace T4Dungeon.Game.Utils
{
    public static class TerminalGuard
    {
        // Check if we're in the browser once to save performance
        private static readonly bool IsBrowser = RuntimeInformation.IsOSPlatform(OSPlatform.Create("BROWSER"));

        public static void SetColor(ConsoleColor color)
        {
            if (IsBrowser)
            {
                // ANSI codes for colors that xterm.js understands 
                string code = color switch
                {
                    ConsoleColor.Red => "\x1b[31m",
                    ConsoleColor.Green => "\x1b[32m",
                    ConsoleColor.Yellow => "\x1b[33m",
                    ConsoleColor.Magenta => "\x1b[35m",
                    ConsoleColor.Cyan => "\x1b[36m",
                    ConsoleColor.White => "\x1b[37m",
                    ConsoleColor.Gray => "\x1b[90m",
                    ConsoleColor.DarkCyan => "\x1b[34m",
                    _ => "\x1b[0m"
                };
                Console.Write(code);
            }
            else
            {
                Console.ForegroundColor = color;
            }
        }

        public static void Reset()
        {
            if (IsBrowser) Console.Write("\x1b[0m");
            else Console.ResetColor();
        }

        public static void Clear()
        {
            if (IsBrowser) Console.Write("\x1b[2J\x1b[H");
            else Console.Clear();
        }

        // Moves the cursor without triggering PlatformNotSupportedException
        public static void SaveCursor() => Console.Write("\x1b[s");
        public static void RestoreCursor() => Console.Write("\x1b[u");
    }
}