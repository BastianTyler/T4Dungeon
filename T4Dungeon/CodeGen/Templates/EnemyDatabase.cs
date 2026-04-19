using System.Collections.Generic;

namespace T4Dungeon.Generated;

public class EnemyDef
{
    public EnemyId Id { get; set; }
    public string Name { get; set; }
    public int HP { get; set; }
    public int Attack { get; set; }
}

public static class EnemyDatabase
{
    public static readonly Dictionary<EnemyId, EnemyDef> Enemies = new()
    {
        {
            EnemyId.Slime,
            new EnemyDef
            {
                Id = EnemyId.Slime,
                Name = "Slime",
                HP = 20,
                Attack = 5
            }
        },
        {
            EnemyId.Goblin,
            new EnemyDef
            {
                Id = EnemyId.Goblin,
                Name = "Goblin",
                HP = 45,
                Attack = 12
            }
        },
        {
            EnemyId.Orc,
            new EnemyDef
            {
                Id = EnemyId.Orc,
                Name = "Orc",
                HP = 100,
                Attack = 25
            }
        }
    };
}