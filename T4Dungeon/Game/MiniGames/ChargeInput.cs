using System.Runtime.InteropServices;
using T4Dungeon.Game.MiniGames;
using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

internal class ChargeInput : IMiniGame
{
    // Necessary to detect when the key is physically released
    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);
    private const int VK_SPACE = 0x20;

    public bool Run(MoveDef move) => Execute(move.TimeLimit);
    public bool RunStep(SkillStep step) => Execute(step.Time);

    private bool Execute(int timeLimitMs)
    {
        int barLength = 30;
        double targetStart = 0.75;
        double targetEnd = 0.95;
        double power = 0;
        bool hasStartedCharging = false;

        DateTime? startTime = null;
        var timeoutWatch = System.Diagnostics.Stopwatch.StartNew();

        // Initial prompt
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write($"  {TextColor.Yellow}HOLD [SPACE] TO CHARGE...{TextColor.Reset}");

        while (timeoutWatch.ElapsedMilliseconds < timeLimitMs)
        {
            bool isKeyDown = (GetAsyncKeyState(VK_SPACE) & 0x8000) != 0;

            if (isKeyDown)
            {
                if (!hasStartedCharging)
                {
                    hasStartedCharging = true;
                    startTime = DateTime.Now;
                }

                // Oscillate while held
                double elapsed = (DateTime.Now - startTime.Value).TotalMilliseconds;
                power = (Math.Sin(elapsed / 800 * Math.PI * 2 - Math.PI / 2) + 1) / 2;

                Render(power, barLength, targetStart, targetEnd);
            }
            else if (hasStartedCharging)
            {
                // Key was released! Check for success
                bool success = power >= targetStart && power <= targetEnd;
                ClearLine();
                return success;
            }

            Thread.Sleep(15);
        }

        ClearLine();
        return false; // Timed out
    }

    private void Render(double power, int length, double tStart, double tEnd)
    {
        int markerPos = (int)(power * length);
        int zoneStart = (int)(tStart * length);
        int zoneEnd = (int)(tEnd * length);

        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write($"  CHARGING: {TextColor.Cyan}▐{TextColor.Reset}");

        for (int i = 0; i < length; i++)
        {
            if (i == markerPos)
                Console.Write($"{TextColor.White}█{TextColor.Reset}");
            else if (i >= zoneStart && i <= zoneEnd)
                Console.Write($"{TextColor.Magenta}▒{TextColor.Reset}"); // Target Zone in Magenta[cite: 11]
            else
                Console.Write($"{TextColor.Gray}░{TextColor.Reset}");
        }

        Console.Write($"{TextColor.Cyan}▌{TextColor.Reset}  {TextColor.Green}RELEASE!{TextColor.Reset}          ");
    }

    private void ClearLine()
    {
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop);
    }
}