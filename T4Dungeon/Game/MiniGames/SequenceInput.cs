using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.MiniGames
{
    internal class SequenceInput : IMiniGame
    {
        public bool Run(MoveDef move) => Execute(move.Key, move.Count, move.TimeLimit);
        public bool RunStep(SkillStep step) => Execute(step.Key, step.Goal, step.Time);

        private bool Execute(char key, int count, int timeLimitPerPress)
        {
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(
                    $"{TextColor.Gray}({i + 1}/{count}){TextColor.Reset} " +
                    $"QUICK! PRESS {TextColor.Yellow}{TextColor.Bold}{char.ToUpper(key)}{TextColor.Reset}!"
                );

                bool success = new TimedInput().Run(new MoveDef
                {
                    Key = key,
                    TimeLimit = timeLimitPerPress
                });

                if (!success)
                    return false;

                Thread.Sleep(100);
            }

            return true;
        }
    }
}