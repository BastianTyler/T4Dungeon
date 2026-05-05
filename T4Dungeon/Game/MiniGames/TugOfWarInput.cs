using T4Dungeon.Game.MiniGames;
using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

internal class TugOfWarInput : IMiniGame
{
    public bool Run(MoveDef move) => Execute(move.Key, move.TimeLimit, move.PullStrength);

    public bool RunStep(SkillStep step) => Execute(step.Key, step.Time, 12);

    private bool Execute(char key, int timeLimitMs, double xmlStrength)
    {
        double balance = 0.5;

        double enemyStrength = xmlStrength / 1000.0;

        double playerPull = 0.045;

        var start = DateTime.Now;

        while ((DateTime.Now - start).TotalMilliseconds < timeLimitMs)
        {
            balance -= enemyStrength;

            if (Console.KeyAvailable)
            {
                if (char.ToLower(Console.ReadKey(true).KeyChar) == char.ToLower(key))
                    balance += playerPull;
            }

            balance = Math.Clamp(balance, 0, 1);

            if (balance >= 1.0) { ClearLine(); return true; }
            if (balance <= 0.0) { ClearLine(); return false; }

            Render(balance, key);
            Thread.Sleep(20);
        }

        ClearLine();
        return false;
    }

    private void Render(double bal, char key)
    {
        int width = 30;
        int marker = (int)(bal * width);
        Console.SetCursorPosition(0, Console.CursorTop);

        Console.Write($"  {TextColor.Red}◄ ENEMY{TextColor.Reset} ");

        for (int i = 0; i < width; i++)
        {
            if (i == marker)
                Console.Write($"{TextColor.White}█{TextColor.Reset}");
            else
            {
                if (i < marker)
                    Console.Write($"{TextColor.Red}▒{TextColor.Reset}");
                else
                    Console.Write($"{TextColor.Green}░{TextColor.Reset}");
            }
        }

        Console.Write($" {TextColor.Green}YOU ►{TextColor.Reset}");
        Console.Write($"   {TextColor.Cyan}{TextColor.Bold}MASH [{char.ToUpper(key)}]{TextColor.Reset}   ");
    }

    private void ClearLine()
    {
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop);
    }
}