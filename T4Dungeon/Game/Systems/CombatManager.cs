using T4Dungeon.Game.Models;
using T4Dungeon.Generated;

public class CombatManager
{
    private static readonly List<(EnemyId id, int weight)> _spawnTable = new()
    {
        (EnemyId.GreenSlime, 100),
        (EnemyId.BlueSlime, 60),
        (EnemyId.AmberSlime, 30),
        (EnemyId.RedSlime, 10),
        (EnemyId.Goblin, 20),
        (EnemyId.Orc, 10),
    };

    public Enemy CreateRandomEnemy()
    {
        int total = _spawnTable.Sum(e => e.weight);
        int roll = Random.Shared.Next(total);

        int cumulative = 0;
        foreach (var (id, weight) in _spawnTable)
        {
            cumulative += weight;
            if (roll < cumulative)
                return new Enemy(id);
        }

        return new Enemy(_spawnTable[0].id);
    }
}