using T4Dungeon.Game.MiniGames;
using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Systems
{
    public class MinigameSystem
    {
        // =========================
        // ROUTING
        // =========================

        private IMiniGame GetGame(string type) => type switch
        {
            "Timed" => new TimedInput(),
            "Mash" => new MashInput(),
            "SweetSpot" => new SweetSpotInput(),
            "Sequence" => new SequenceInput(),
            "ChainedHitBar" => new ChainedHitBarInput(),
            "Charge" => new ChargeInput(),
            "Balance" => new BalanceInput(),
            "TugOfWar" => new TugOfWarInput(),
            _ => null
        };

        private MoveDef StepToMove(MoveStep step) => new MoveDef
        {
            Type = step.Type,
            Key = step.Key,
            Goal = step.Goal,
            Count = step.Count,
            TimeLimit = step.TimeLimit,
            Target = 0.5,
            Threshold = 0.1,
            ChainedHitBarPositions = step.ChainedHitBarPositions,
            PullStrength = step.PullStrength
        };

        // =========================
        // ENEMY MOVES
        // =========================

        // Single move or multi-step move
        public bool Execute(MoveDef move, GameLogSystem log = null)
        {
            if (move.Steps != null && move.Steps.Count > 0)
            {
                foreach (var step in move.Steps)
                {
                    bool success = GetGame(step.Type)?.Run(StepToMove(step)) ?? true;

                    if (!success)
                    {
                        if (log != null && !string.IsNullOrEmpty(step.FailMsg))
                            log.Add($"{TextColor.Red}{step.FailMsg}{TextColor.Reset}");
                        return false;
                    }
                }
                return true;
            }

            return GetGame(move.Type)?.Run(move) ?? true;
        }

        // =========================
        // SKILLS
        // =========================

        public bool ExecuteSkill(SkillDef skill, GameLogSystem log)
        {
            foreach (var step in skill.Steps)
            {
                bool success = GetGame(step.Type)?.RunStep(step) ?? true;

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