using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.MiniGames
{
    internal class SweetSpotInput : IMiniGame
    {
        public bool Run(MoveDef move) => Execute(move.Key, move.Target, move.Threshold);
        public bool RunStep(SkillStep step) => Execute(step.Key, 0.5, 0.1); // defaults since SkillStep has no Target/Threshold

        private bool Execute(char stopKey, double targetPercent, double threshold)
        {
            while (Console.KeyAvailable) Console.ReadKey(true);

            var start = DateTime.Now;
            int totalBarLength = 30;

            int targetIndex = (int)(targetPercent * totalBarLength);
            int halfZoneWidth = Math.Max(1, (int)(threshold * totalBarLength));
            int zoneStart = Math.Max(0, targetIndex - halfZoneWidth);
            int zoneEnd = Math.Min(totalBarLength - 1, targetIndex + halfZoneWidth);

            Console.WriteLine();
            int cursorTop = Console.CursorTop;
            Console.WriteLine();

            while (true)
            {
                double elapsed = (DateTime.Now - start).TotalMilliseconds;
                double progress = (Math.Sin(elapsed / 1500 * Math.PI * 2) + 1) / 2;
                int markerIndex = (int)(progress * totalBarLength);

                Console.SetCursorPosition(0, cursorTop);
                Console.Write($"  {TextColor.Cyan}▐{TextColor.Reset}");

                for (int i = 0; i < totalBarLength; i++)
                {
                    if (i == markerIndex)
                        Console.Write($"{TextColor.Cyan}{TextColor.Bold}▌{TextColor.Reset}");
                    else if (i == targetIndex)
                        Console.Write($"{TextColor.White}{TextColor.Bold}▓{TextColor.Yellow}");
                    else if (i >= zoneStart && i <= zoneEnd)
                        Console.Write($"{TextColor.Green}▓{TextColor.Reset}");
                    else
                        Console.Write($"{TextColor.Gray}▒{TextColor.Reset}");
                }

                Console.Write($"{TextColor.Cyan}▌{TextColor.Reset}  PRESS {TextColor.Yellow}{TextColor.Bold}{char.ToUpper(stopKey)}{TextColor.Reset}!   ");

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).KeyChar;
                    if (char.ToLower(key) == char.ToLower(stopKey))
                    {
                        bool hit = markerIndex >= zoneStart && markerIndex <= zoneEnd;
                        ClearLine(cursorTop);
                        return hit;
                    }
                }

                Thread.Sleep(15);
            }
        }

        private void ClearLine(int row)
        {
            Console.SetCursorPosition(0, row);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, row);
        }
    }
}