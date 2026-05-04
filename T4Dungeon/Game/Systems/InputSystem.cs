using System;
using System.Threading;
using T4Dungeon.Game.Models;

public class InputSystem
{
    public int GetSelection(int maxOption)
    {
        int blinkStage = 0; int promptLine = Console.CursorTop;
        while (true)
        {
            while (!Console.KeyAvailable)
            {
                Console.SetCursorPosition(0, promptLine);
                Console.ForegroundColor = (blinkStage % 2 == 0) ? ConsoleColor.DarkCyan : ConsoleColor.Cyan;
                Console.Write($" >> CHOOSE AN OPTION [1-{maxOption}] <<   "); Console.ResetColor();
                Thread.Sleep(400); blinkStage++;
            }
            var key = Console.ReadKey(true);
            if (char.IsDigit(key.KeyChar))
            {
                int value = key.KeyChar - '1';
                if (value >= 0 && value < maxOption)
                {
                    Console.SetCursorPosition(0, promptLine); Console.Write(new string(' ', Console.WindowWidth)); // Clear prompt
                    Console.SetCursorPosition(0, promptLine); return value;
                }
            }
        }
    }

    public int GetSelection(UIContext ui, GameLogSystem log)
    {
        while (true)
        {
            int choice = GetSelection(ui.Options.Count); // Calls the original method
            if (ui.Options[choice].IsImplemented) return choice;
            log.Add("Option not implemented.");
        }
    }

}