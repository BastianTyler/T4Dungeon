using T4Dungeon.Game.MiniGames;
using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Systems
{
    public class MinigameSystem
    {
        public bool Execute(MoveDef move)
        {
            IMiniGame game = move.Type switch
            {
                "Timed" => new TimedInput(),
                "Mash" => new MashInput(),
                "SweetSpot" => new SweetSpotInput(),
                "Sequence" => new SequenceInput(),
                "ChaineHitBar" => new ChainedHitBarInput(),
                _ => null
            };

            return game?.Run(move) ?? true;
        }

        public bool ExecuteSkill(SkillDef skill, GameLogSystem log)
        {
            foreach (var step in skill.Steps)
            {
                IMiniGame game = step.Type switch
                {
                    "Timed" => new TimedInput(),
                    "Mash" => new MashInput(),
                    "SweetSpot" => new SweetSpotInput(),
                    "Sequence" => new SequenceInput(),
                    "ChainedHitBar" => new ChainedHitBarInput(),
                    _ => null
                };

                bool success = game?.RunStep(step) ?? true;

                if (!success)
                {
                    log.Add($"{TextColor.Red}{step.FailMsg}{TextColor.Reset}");
                    return false;
                }
            }

            return true;
        }
    }

}