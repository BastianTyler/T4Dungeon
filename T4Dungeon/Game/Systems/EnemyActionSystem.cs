using T4Dungeon.Game.Models;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Systems
{
    public class EnemyActionSystem
    {
        public MoveDef SelectMove(Enemy enemy)
        {
            return enemy.Moves[Random.Shared.Next(enemy.Moves.Count)];
        }
    }
}