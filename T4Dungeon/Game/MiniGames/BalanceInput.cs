using T4Dungeon.Game.MiniGames;
using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

internal class BalanceInput : IMiniGame
{
    // Use the PullStrength property from MoveDef
    public bool Run(MoveDef move) => Execute(move.TimeLimit, move.PullStrength);

    // Defaulting to 18 (0.018) for SkillSteps
    public bool RunStep(SkillStep step) => Execute(step.Time, 18.0);

    private bool Execute(int durationMs, double xmlStrength)
    {
        double pos = 0.5;

        // Scaling: XML Value (e.g., 18.0) / 1000.0 = 0.018 base force
        double monsterForce = xmlStrength / 1000.0;

        var start = DateTime.Now;
        Random rnd = new Random();
        bool pushingRight = rnd.Next(0, 2) == 0;

        while ((DateTime.Now - start).TotalMilliseconds < durationMs)
        {
            // Apply the scaled force
            if (pushingRight) pos += monsterForce;
            else pos -= monsterForce;

            // Chance to switch directions
            if (rnd.Next(0, 100) < 3) pushingRight = !pushingRight;

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                // Direct response jumps
                if (key == ConsoleKey.LeftArrow) pos -= 0.10;
                if (key == ConsoleKey.RightArrow) pos += 0.10;
            }

            // Failure: Entering the "Dead Zone" (Outer 10%)
            if (pos <= 0.10 || pos >= 0.90)
            {
                ClearLine();
                return false;
            }

            Render(pos);
            Thread.Sleep(35);
        }

        ClearLine();
        return true;
    }

    private void Render(double pos)
    {
        int width = 40;
        int cursor = (int)(Math.Clamp(pos, 0, 1) * width);
        Console.SetCursorPosition(0, Console.CursorTop);

        string leftPrompt = pos > 0.6 ? $"{TextColor.Yellow}◄ PUSH LEFT{TextColor.Reset}" : "           ";
        string rightPrompt = pos < 0.4 ? $"{TextColor.Yellow}PUSH RIGHT ►{TextColor.Reset}" : "            ";

        Console.Write($"{leftPrompt} ");

        for (int i = 0; i < width; i++)
        {
            if (i == cursor)
            {
                Console.Write($"{TextColor.Cyan}▲{TextColor.Reset}");
            }
            else
            {
                double section = (double)i / width;

                // Dithering for Background Zones[cite: 1]
                if (section < 0.15 || section > 0.85)
                    Console.Write($"{TextColor.Red}▓{TextColor.Reset}"); // Danger
                else if (section < 0.25 || section > 0.75)
                    Console.Write($"{TextColor.Red}▒{TextColor.Reset}"); // Warning
                else if (section < 0.4 || section > 0.6)
                    Console.Write($"{TextColor.Green}░{TextColor.Reset}"); // Stability
                else
                    Console.Write($"{TextColor.Green}█{TextColor.Reset}"); // Safe
            }
        }

        Console.Write($" {rightPrompt} ");
    }

    private void ClearLine()
    {
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop);
    }
}