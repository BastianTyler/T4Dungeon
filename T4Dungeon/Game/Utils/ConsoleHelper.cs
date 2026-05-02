namespace T4Dungeon.Game.Utils
{
    public static class ConsoleHelper // or add to ConsoleRenderer
    {
        /// <summary>
        /// Clears a specific line in the console for clean output.
        /// </summary>
        public static void ClearLine(int row)
        {
            // Ensure we don't try to write outside the buffer if the window was resized
            if (row < 0 || row >= Console.BufferHeight) return;

            Console.SetCursorPosition(0, row);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, row);
        }
    }
}