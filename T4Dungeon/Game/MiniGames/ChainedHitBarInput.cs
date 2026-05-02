using T4Dungeon.Game.Models;
using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.MiniGames
{
    internal class ChainedHitBarInput : IMiniGame
    {
        public bool Run(MoveDef move) => Execute(move.ChainedHitBarPositions, move.TimeLimit);

        public bool RunStep(SkillStep step) => Execute(step.ChainedHitBarPositions, step.Time);

        private bool Execute(string positions, int timeLimit)
        {
            var targets = positions.Split(',')
                .Select(s => new SweetspotTarget
                {
                    Center = float.Parse(s.Trim()),
                    Width = 0.08f,
                    IsHit = false
                }).ToList();

            DateTime startTime = DateTime.Now;
            float cursor = 0f;

            while (Console.KeyAvailable) Console.ReadKey(true);

            while (cursor < 1.0f)
            {
                float elapsed = (float)(DateTime.Now - startTime).TotalMilliseconds;
                cursor = elapsed / timeLimit;

                Render(cursor, targets, 40);

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Spacebar)
                    {
                        for (int i = 0; i < targets.Count; i++)
                        {
                            var t = targets[i];
                            if (!t.IsHit && Math.Abs(cursor - t.Center) < t.Width / 2)
                            {
                                t.IsHit = true;
                                targets[i] = t;
                                break;
                            }
                        }
                    }
                }

                Thread.Sleep(15);
            }

            return targets.All(t => t.IsHit);
        }

        private void Render(float cursor, List<SweetspotTarget> targets, int width)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write($"{TextColor.Cyan}  [ SPACE ]{TextColor.Reset} to HIT!  [");

            for (int i = 0; i < width; i++)
            {
                float pos = (float)i / width;
                bool isCursor = Math.Abs(pos - cursor) < (1.5f / width);
                var target = targets.FirstOrDefault(t => Math.Abs(pos - t.Center) < t.Width / 2);

                if (isCursor)
                    Console.Write($"{TextColor.White}|{TextColor.Reset}");
                else if (target.Center != 0)
                    Console.Write(target.IsHit
                        ? $"{TextColor.Green}█{TextColor.Reset}"
                        : $"{TextColor.Red}▒{TextColor.Reset}");
                else
                    Console.Write($"{TextColor.Gray}-{TextColor.Reset}");
            }

            int hits = targets.Count(t => t.IsHit);
            Console.Write($"]  {TextColor.Yellow}{hits}/{targets.Count}{TextColor.Reset}  ");
        }
    }
}