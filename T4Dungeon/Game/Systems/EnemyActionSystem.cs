using T4Dungeon.Game.Models;
using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

public class EnemyActionSystem
{
    public MoveDef SelectMove(Enemy enemy)
    {
        var moves = GetCurrentMoves(enemy);
        return moves[Random.Shared.Next(moves.Count)];
    }

    public void CheckStageTransition(Enemy enemy, GameLogSystem log)
    {
        if (!enemy.HasStages) return;

        var targetStage = enemy.Def.Stages
            .Where(s => enemy.HP <= s.HPThreshold)
            .OrderBy(s => s.HPThreshold)
            .FirstOrDefault();

        if (targetStage == null || targetStage.Id == enemy.CurrentStageId)
            return;

        enemy.CurrentStageId = targetStage.Id;

        log.Add($"{TextColor.Red}{enemy.Name} — {targetStage.Label}!{TextColor.Reset}",
            waitForKey: true);

        if (targetStage.OnEnter != null)
        {
            enemy.Attack += targetStage.OnEnter.Attack;
            enemy.Defense += targetStage.OnEnter.Defense;
        }
    }

    private List<MoveDef> GetCurrentMoves(Enemy enemy)
    {
        if (!enemy.HasStages)
            return enemy.Moves;

        if (enemy.CurrentStageId == -1)
            return enemy.Moves;

        var stage = enemy.Def.Stages
            .FirstOrDefault(s => s.Id == enemy.CurrentStageId);

        return stage?.Moves?.Count > 0 ? stage.Moves : enemy.Moves;
    }
}