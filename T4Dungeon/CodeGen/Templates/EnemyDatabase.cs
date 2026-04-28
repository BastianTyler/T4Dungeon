using System.Collections.Generic;

namespace T4Dungeon.Generated;

public class MoveDef
{
    public string Name { get; set; }
    public string Type { get; set; }
    public char Key { get; set; }
    public int TimeLimit { get; set; }
    public int Goal { get; set; }
    public int Count { get; set; }
    public double Target { get; set; }
    public double Threshold { get; set; }
    public string ChainedHitBarPositions { get; set; }
}

public class LootItemDef {
    public ItemId Id { get; set; }
    public double Chance { get; set; }
}

public class EnemyDef
{
    public EnemyId Id { get; set; }
    public string Name { get; set; }
    public int HP { get; set; }
    public int MaxHp {get; set; }
    public int Attack { get; set; }
    public List<MoveDef> Moves { get; set; } = new();
    public int MinGold { get; set; }
    public int MaxGold { get; set; }
    public List<LootItemDef> LootTable { get; set; } = new();
}

public static class EnemyDatabase
{
    public static readonly Dictionary<EnemyId, EnemyDef> Enemies = new()
    {
        {
            EnemyId.GreenSlime,
            new EnemyDef
            {
                Id = EnemyId.GreenSlime,
                Name = "Green Slime",
                HP = 20,
                MaxHp = 20,
                Attack = 5,
                MinGold = 2,
                MaxGold = 6,
                Moves = new List<MoveDef> 
                {
                    new MoveDef 
                    { 
                        Name = "Slime Ram",
                        Type = "Timed",
                        Key = 'D',
                        TimeLimit = 2500,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                    new LootItemDef 
                    { 
                        Id = (ItemId)2001, 
                        Chance = 0.2 
                    }
                } // End of LootTable List
            }
        },
        {
            EnemyId.Goblin,
            new EnemyDef
            {
                Id = EnemyId.Goblin,
                Name = "Goblin",
                HP = 45,
                MaxHp = 45,
                Attack = 12,
                MinGold = 10,
                MaxGold = 20,
                Moves = new List<MoveDef> 
                {
                    new MoveDef 
                    { 
                        Name = "Quick Stab",
                        Type = "Timed",
                        Key = 'D',
                        TimeLimit = 1200,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1
                    },
                    new MoveDef 
                    { 
                        Name = "Dagger Throw",
                        Type = "SweetSpot",
                        Key = 'R',
                        TimeLimit = 2000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                    new LootItemDef 
                    { 
                        Id = (ItemId)1005, 
                        Chance = 0.05 
                    }
                } // End of LootTable List
            }
        },
        {
            EnemyId.Orc,
            new EnemyDef
            {
                Id = EnemyId.Orc,
                Name = "Orc",
                HP = 100,
                MaxHp = 100,
                Attack = 25,
                MinGold = 35,
                MaxGold = 60,
                Moves = new List<MoveDef> 
                {
                    new MoveDef 
                    { 
                        Name = "Crushing Club",
                        Type = "Mash",
                        Key = 'B',
                        TimeLimit = 3000,
                        Goal = 15,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1
                    },
                    new MoveDef 
                    { 
                        Name = "Double Swing",
                        Type = "Sequence",
                        Key = 'B',
                        TimeLimit = 2000,
                        Goal = 0,
                        Count = 2,
                        Target = 0.5,
                        Threshold = 0.1
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                    new LootItemDef 
                    { 
                        Id = (ItemId)1001, 
                        Chance = 0.1 
                    },
                    new LootItemDef 
                    { 
                        Id = (ItemId)2001, 
                        Chance = 0.15 
                    }
                } // End of LootTable List
            }
        },
        {
            EnemyId.BlueSlime,
            new EnemyDef
            {
                Id = EnemyId.BlueSlime,
                Name = "Blue Slime",
                HP = 30,
                MaxHp = 30,
                Attack = 8,
                MinGold = 5,
                MaxGold = 10,
                Moves = new List<MoveDef> 
                {
                    new MoveDef 
                    { 
                        Name = "Quick Leap",
                        Type = "Timed",
                        Key = 'W',
                        TimeLimit = 1500,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                    new LootItemDef 
                    { 
                        Id = (ItemId)2001, 
                        Chance = 0.3 
                    }
                } // End of LootTable List
            }
        },
        {
            EnemyId.AmberSlime,
            new EnemyDef
            {
                Id = EnemyId.AmberSlime,
                Name = "Amber Slime",
                HP = 50,
                MaxHp = 50,
                Attack = 12,
                MinGold = 12,
                MaxGold = 18,
                Moves = new List<MoveDef> 
                {
                    new MoveDef 
                    { 
                        Name = "Amber Crush",
                        Type = "Sequence",
                        Key = 'S',
                        TimeLimit = 1000,
                        Goal = 0,
                        Count = 3,
                        Target = 0.5,
                        Threshold = 0.1
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.RedSlime,
            new EnemyDef
            {
                Id = EnemyId.RedSlime,
                Name = "Red Slime",
                HP = 65,
                MaxHp = 65,
                Attack = 18,
                MinGold = 20,
                MaxGold = 35,
                Moves = new List<MoveDef> 
                {
                    new MoveDef 
                    { 
                        Name = "Heavy Bash",
                        Type = "Sequence",
                        Key = 'B',
                        TimeLimit = 800,
                        Goal = 0,
                        Count = 2,
                        Target = 0.5,
                        Threshold = 0.1
                    },
                    new MoveDef 
                    { 
                        Name = "Core Burst",
                        Type = "SweetSpot",
                        Key = 'X',
                        TimeLimit = 2000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.05
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        }
    };
}