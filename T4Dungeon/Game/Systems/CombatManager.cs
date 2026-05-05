using System.Timers;
using T4Dungeon.Game.Models;
using T4Dungeon.Generated;

public class CombatManager
{
    //TIER 1: Slime Garden
    private static readonly List<(EnemyId id, int weight)> _spawnTableTierOne = new()
    {
        (EnemyId.GreenSlime, 80),
        (EnemyId.BlueSlime, 70),
        (EnemyId.AmberSlime, 40),
        (EnemyId.RedSlime, 20),
        (EnemyId.RoyalSlime, 10),
    };

    // TIER 2: Undead Halls
    private static readonly List<(EnemyId id, int weight)> _tierTwo = new()
    {
        (EnemyId.AmberSlime, 80),
        (EnemyId.RedSlime, 60),
        (EnemyId.Orc, 50),
        (EnemyId.SkeletonGuard, 40),
        (EnemyId.StoneCrab, 30),
        (EnemyId.Harpy, 25),
        (EnemyId.Mimic, 10), // Rare encounter
    };

    // TIER 3: Elite Challengers 
    private static readonly List<(EnemyId id, int weight)> _tierThree = new()
    {
        (EnemyId.StoneGolem, 60),
        (EnemyId.ShadowStalker, 55),
        (EnemyId.IronKnight, 50),
        (EnemyId.CaveTroll, 40),
        (EnemyId.DarkPriest, 35),
        (EnemyId.CursedSpecter, 30),
        (EnemyId.ShadowAssassin, 20),
        (EnemyId.BeholderSpawn, 15),
    };

    // TIER 4: 
    private static readonly List<(EnemyId id, int weight)> _tierFour = new()
    {
        (EnemyId.FrostGiant, 50),
        (EnemyId.IronGolem, 45),
        (EnemyId.Chimera, 40),
        (EnemyId.LichApprentice, 35),
        (EnemyId.AncientConstruct, 15),
        (EnemyId.TheNamelessKnight, 5), // Extremely rare elite
    };

    public Enemy CreateRandomEnemy(int tier = 1)
    {

        var table = tier switch
        {
            1 => _spawnTableTierOne,
            2 => _tierTwo,
            3 => _tierThree,
            4 => _tierFour,
            _ => _spawnTableTierOne
        };

        int total = table.Sum(e => e.weight);
        int roll = Random.Shared.Next(total);

        int cumulative = 0;
        foreach (var (id, weight) in table)
        {
            cumulative += weight;
            if (roll < cumulative)
                return new Enemy(id);
        }

        return new Enemy(table[0].id);
    }

    public Enemy CreateEnemy(EnemyId id)
    {
        return new Enemy(id);
    }
}