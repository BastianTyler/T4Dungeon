using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.MiniGames
{
    internal class TimedInput : IMiniGame
    {
        public bool Run(MoveDef move) => Execute(move.Key, move.TimeLimit);
        public bool RunStep(SkillStep step) => Execute(step.Key, step.Time);

        private bool Execute(char expectedKey, int timeLimitMs)
        {
            while (Console.KeyAvailable) Console.ReadKey(intercept: true);

            var start = DateTime.Now;
            int totalBarLength = 30;

            Console.WriteLine();
            int cursorTop = Console.CursorTop;
            Console.WriteLine();

            while ((DateTime.Now - start).TotalMilliseconds < timeLimitMs)
            {
                double elapsed = (DateTime.Now - start).TotalMilliseconds;
                double percent = elapsed / timeLimitMs;
                int filled = (int)(percent * totalBarLength);

                string fillColor = percent < 0.5 ? TextColor.Green
                                 : percent < 0.8 ? TextColor.Yellow
                                 : TextColor.Red;

                Console.SetCursorPosition(0, cursorTop);
                Console.Write($"  {TextColor.Cyan}▐{TextColor.Reset}");

                for (int i = 0; i < totalBarLength; i++)
                    Console.Write(i < filled
                        ? $"{fillColor}█{TextColor.Reset}"
                        : $"{TextColor.Gray}░{TextColor.Reset}");

                Console.Write($"{TextColor.Cyan}▌{TextColor.Reset}  PRESS {TextColor.Yellow}{TextColor.Bold}{char.ToUpper(expectedKey)}{TextColor.Reset}!    ");

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).KeyChar;
                    ClearLine(cursorTop);
                    return char.ToLower(key) == char.ToLower(expectedKey);
                }

                Thread.Sleep(20);
            }

            ClearLine(cursorTop);
            return false;
        }

        private void ClearLine(int row)
        {
            Console.SetCursorPosition(0, row);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, row);
        }
    }
}