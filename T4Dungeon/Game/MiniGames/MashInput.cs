using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.MiniGames
{
    internal class MashInput : IMiniGame
    {
        public bool Run(MoveDef move) => Execute(move.Key, move.Goal, move.TimeLimit);
        public bool RunStep(SkillStep step) => Execute(step.Key, step.Goal, step.Time);

        private bool Execute(char keyToMash, int goalStrikes, int timeLimitMs)
        {
            while (Console.KeyAvailable) Console.ReadKey(true);

            var start = DateTime.Now;
            int currentStrikes = 0;
            int totalBarLength = 30;

            Console.WriteLine();
            int cursorTop = Console.CursorTop;
            Console.WriteLine();

            while ((DateTime.Now - start).TotalMilliseconds < timeLimitMs)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).KeyChar;
                    if (char.ToLower(key) == char.ToLower(keyToMash))
                        currentStrikes++;
                }

                if (currentStrikes >= goalStrikes)
                {
                    ClearLine(cursorTop);
                    return true;
                }

                int filled = (int)((double)currentStrikes / goalStrikes * totalBarLength);

                Console.SetCursorPosition(0, cursorTop);
                Console.Write($"  {TextColor.Cyan}▐{TextColor.Reset}");

                for (int i = 0; i < totalBarLength; i++)
                    Console.Write(i < filled
                        ? $"{TextColor.Green}█{TextColor.Reset}"
                        : $"{TextColor.Gray}░{TextColor.Reset}");

                Console.Write(
                    $"{TextColor.Cyan}▌{TextColor.Reset}  MASH " +
                    $"{TextColor.Yellow}{TextColor.Bold}{char.ToUpper(keyToMash)}{TextColor.Reset}!" +
                    $"  {TextColor.Gray}({currentStrikes}/{goalStrikes}){TextColor.Reset}   "
                );

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