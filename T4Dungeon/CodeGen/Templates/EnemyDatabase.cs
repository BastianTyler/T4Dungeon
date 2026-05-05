using System;
using System.Collections.Generic;

namespace T4Dungeon.Generated;

public class MoveStep
{
    public string Type { get; set; }
    public char Key { get; set; }
    public int Goal { get; set; }
    public int TimeLimit { get; set; }
    public string FailMsg { get; set; }
    public string ChainedHitBarPositions { get; set; }
    public double PullStrength { get; set; }
    public int Count { get; set; }
}

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
    public double PullStrength { get; set; }
    public List<MoveStep> Steps { get; set; } = new();
}

public class LootItemDef
{
    public ItemId Id { get; set; }
    public double Chance { get; set; }
}

public class EnemyDef
{
    public EnemyId Id { get; set; }
    public string Name { get; set; }
    public int HP { get; set; }
    public int MaxHp { get; set; }
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
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
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
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
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
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
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
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
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
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
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
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
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
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
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
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
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
                        Threshold = 0.05,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.StoneGolem,
            new EnemyDef
            {
                Id = EnemyId.StoneGolem,
                Name = "Stone Golem",
                HP = 150,
                MaxHp = 150,
                Attack = 30,
                MinGold = 50,
                MaxGold = 100,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Earthquake",
                        Type = "Balance",
                        Key = 'D',
                        TimeLimit = 4000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 22,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                    new LootItemDef
                    {
                        Id = (ItemId)1001,
                        Chance = 0.2
                    }
                } // End of LootTable List
            }
        },
        {
            EnemyId.ShadowStalker,
            new EnemyDef
            {
                Id = EnemyId.ShadowStalker,
                Name = "Shadow Stalker",
                HP = 80,
                MaxHp = 80,
                Attack = 20,
                MinGold = 30,
                MaxGold = 50,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Shadow Bind",
                        Type = "TugOfWar",
                        Key = 'R',
                        TimeLimit = 5000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 18,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                    new LootItemDef
                    {
                        Id = (ItemId)2001,
                        Chance = 0.25
                    }
                } // End of LootTable List
            }
        },
        {
            EnemyId.IronKnight,
            new EnemyDef
            {
                Id = EnemyId.IronKnight,
                Name = "Iron Knight",
                HP = 120,
                MaxHp = 120,
                Attack = 35,
                MinGold = 40,
                MaxGold = 80,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Grand Overdrive",
                        Type = "Charge",
                        Key = 'D',
                        TimeLimit = 3000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    },
                    new MoveDef
                    {
                        Name = "Hilt Bump",
                        Type = "Timed",
                        Key = 'W',
                        TimeLimit = 800,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                    new LootItemDef
                    {
                        Id = (ItemId)1003,
                        Chance = 0.15
                    }
                } // End of LootTable List
            }
        },
        {
            EnemyId.SkeletonGuard,
            new EnemyDef
            {
                Id = EnemyId.SkeletonGuard,
                Name = "Skeleton Guard",
                HP = 85,
                MaxHp = 85,
                Attack = 22,
                MinGold = 25,
                MaxGold = 45,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Bone Rattle",
                        Type = "Sequence",
                        Key = 'W',
                        TimeLimit = 1200,
                        Goal = 0,
                        Count = 4,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    },
                    new MoveDef
                    {
                        Name = "Shield Bash",
                        Type = "Timed",
                        Key = 'S',
                        TimeLimit = 900,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.CursedSpecter,
            new EnemyDef
            {
                Id = EnemyId.CursedSpecter,
                Name = "Cursed Specter",
                HP = 70,
                MaxHp = 70,
                Attack = 28,
                MinGold = 60,
                MaxGold = 90,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Soul Drain",
                        Type = "TugOfWar",
                        Key = 'F',
                        TimeLimit = 4000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 25,
                        Steps = new List<MoveStep>
                        {
                        }
                    },
                    new MoveDef
                    {
                        Name = "Ethereal Strike",
                        Type = "SweetSpot",
                        Key = 'Z',
                        TimeLimit = 2000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.8,
                        Threshold = 0.04,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.WaspSwarm,
            new EnemyDef
            {
                Id = EnemyId.WaspSwarm,
                Name = "Wasp Swarm",
                HP = 35,
                MaxHp = 35,
                Attack = 10,
                MinGold = 8,
                MaxGold = 14,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Stinger Barrage",
                        Type = "Sequence",
                        Key = 'W',
                        TimeLimit = 900,
                        Goal = 0,
                        Count = 3,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.StoneCrab,
            new EnemyDef
            {
                Id = EnemyId.StoneCrab,
                Name = "Stone Crab",
                HP = 110,
                MaxHp = 110,
                Attack = 15,
                MinGold = 20,
                MaxGold = 40,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Bubble Trap",
                        Type = "TugOfWar",
                        Key = 'A',
                        TimeLimit = 4500,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 14,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.KoboldMiner,
            new EnemyDef
            {
                Id = EnemyId.KoboldMiner,
                Name = "Kobold Miner",
                HP = 55,
                MaxHp = 55,
                Attack = 14,
                MinGold = 25,
                MaxGold = 50,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Pickaxe Throw",
                        Type = "SweetSpot",
                        Key = 'S',
                        TimeLimit = 2000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.3,
                        Threshold = 0.08,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.Mimic,
            new EnemyDef
            {
                Id = EnemyId.Mimic,
                Name = "Mimic",
                HP = 90,
                MaxHp = 90,
                Attack = 25,
                MinGold = 100,
                MaxGold = 200,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Chomp",
                        Type = "Mash",
                        Key = 'C',
                        TimeLimit = 2500,
                        Goal = 20,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    },
                    new MoveDef
                    {
                        Name = "Sticky Tongue",
                        Type = "TugOfWar",
                        Key = 'T',
                        TimeLimit = 4000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 20,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.EarthElemental,
            new EnemyDef
            {
                Id = EnemyId.EarthElemental,
                Name = "Earth Elemental",
                HP = 140,
                MaxHp = 140,
                Attack = 28,
                MinGold = 45,
                MaxGold = 75,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Fissure",
                        Type = "Balance",
                        Key = 'D',
                        TimeLimit = 5000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 19,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.Harpy,
            new EnemyDef
            {
                Id = EnemyId.Harpy,
                Name = "Harpy",
                HP = 75,
                MaxHp = 75,
                Attack = 20,
                MinGold = 30,
                MaxGold = 55,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Screech",
                        Type = "Sequence",
                        Key = 'Q',
                        TimeLimit = 1500,
                        Goal = 0,
                        Count = 5,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.DarkPriest,
            new EnemyDef
            {
                Id = EnemyId.DarkPriest,
                Name = "Dark Priest",
                HP = 60,
                MaxHp = 60,
                Attack = 30,
                MinGold = 40,
                MaxGold = 70,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Void Pulse",
                        Type = "Charge",
                        Key = 'D',
                        TimeLimit = 2500,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.CaveTroll,
            new EnemyDef
            {
                Id = EnemyId.CaveTroll,
                Name = "Cave Troll",
                HP = 180,
                MaxHp = 180,
                Attack = 35,
                MinGold = 60,
                MaxGold = 110,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Great Bash",
                        Type = "Mash",
                        Key = 'M',
                        TimeLimit = 4000,
                        Goal = 25,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.LivingArmor,
            new EnemyDef
            {
                Id = EnemyId.LivingArmor,
                Name = "Living Armor",
                HP = 130,
                MaxHp = 130,
                Attack = 32,
                MinGold = 50,
                MaxGold = 90,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Static Guard",
                        Type = "Charge",
                        Key = 'D',
                        TimeLimit = 3500,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    },
                    new MoveDef
                    {
                        Name = "Shield Slam",
                        Type = "Timed",
                        Key = 'S',
                        TimeLimit = 750,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.BoneNaga,
            new EnemyDef
            {
                Id = EnemyId.BoneNaga,
                Name = "Bone Naga",
                HP = 200,
                MaxHp = 200,
                Attack = 40,
                MinGold = 120,
                MaxGold = 180,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Constrict",
                        Type = "TugOfWar",
                        Key = 'X',
                        TimeLimit = 6000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 24,
                        Steps = new List<MoveStep>
                        {
                        }
                    },
                    new MoveDef
                    {
                        Name = "Tail Whip",
                        Type = "Timed",
                        Key = 'A',
                        TimeLimit = 600,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.FrostGiant,
            new EnemyDef
            {
                Id = EnemyId.FrostGiant,
                Name = "Frost Giant",
                HP = 350,
                MaxHp = 350,
                Attack = 50,
                MinGold = 200,
                MaxGold = 400,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Glacial Crash",
                        Type = "Balance",
                        Key = 'D',
                        TimeLimit = 6000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 30,
                        Steps = new List<MoveStep>
                        {
                        }
                    },
                    new MoveDef
                    {
                        Name = "Ice Boulder",
                        Type = "SweetSpot",
                        Key = 'B',
                        TimeLimit = 2000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.03,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.ShadowAssassin,
            new EnemyDef
            {
                Id = EnemyId.ShadowAssassin,
                Name = "Shadow Assassin",
                HP = 100,
                MaxHp = 100,
                Attack = 45,
                MinGold = 80,
                MaxGold = 130,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Death in Five",
                        Type = "Sequence",
                        Key = 'F',
                        TimeLimit = 800,
                        Goal = 0,
                        Count = 5,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.Wyvern,
            new EnemyDef
            {
                Id = EnemyId.Wyvern,
                Name = "Wyvern",
                HP = 220,
                MaxHp = 220,
                Attack = 38,
                MinGold = 150,
                MaxGold = 250,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Dive Bomb",
                        Type = "Charge",
                        Key = 'D',
                        TimeLimit = 2000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    },
                    new MoveDef
                    {
                        Name = "Poison Claw",
                        Type = "Timed",
                        Key = 'W',
                        TimeLimit = 500,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.BeholderSpawn,
            new EnemyDef
            {
                Id = EnemyId.BeholderSpawn,
                Name = "Beholder Spawn",
                HP = 160,
                MaxHp = 160,
                Attack = 42,
                MinGold = 110,
                MaxGold = 200,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Disintegration Ray",
                        Type = "SweetSpot",
                        Key = 'E',
                        TimeLimit = 2000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.9,
                        Threshold = 0.02,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.IronGolem,
            new EnemyDef
            {
                Id = EnemyId.IronGolem,
                Name = "Iron Golem",
                HP = 400,
                MaxHp = 400,
                Attack = 55,
                MinGold = 300,
                MaxGold = 500,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Piston Punch",
                        Type = "Mash",
                        Key = 'P',
                        TimeLimit = 5000,
                        Goal = 40,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    },
                    new MoveDef
                    {
                        Name = "Steam Blast",
                        Type = "Balance",
                        Key = 'D',
                        TimeLimit = 3500,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 35,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.LichApprentice,
            new EnemyDef
            {
                Id = EnemyId.LichApprentice,
                Name = "Lich Apprentice",
                HP = 140,
                MaxHp = 140,
                Attack = 48,
                MinGold = 180,
                MaxGold = 300,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Life Tap",
                        Type = "TugOfWar",
                        Key = 'L',
                        TimeLimit = 4500,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 28,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.Chimera,
            new EnemyDef
            {
                Id = EnemyId.Chimera,
                Name = "Chimera",
                HP = 280,
                MaxHp = 280,
                Attack = 45,
                MinGold = 250,
                MaxGold = 450,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Triple Strike",
                        Type = "Sequence",
                        Key = 'Z',
                        TimeLimit = 600,
                        Goal = 0,
                        Count = 3,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    },
                    new MoveDef
                    {
                        Name = "Lion Roar",
                        Type = "Mash",
                        Key = 'R',
                        TimeLimit = 3000,
                        Goal = 30,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.AncientConstruct,
            new EnemyDef
            {
                Id = EnemyId.AncientConstruct,
                Name = "Ancient Construct",
                HP = 500,
                MaxHp = 500,
                Attack = 60,
                MinGold = 500,
                MaxGold = 1000,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Reality Warp",
                        Type = "Balance",
                        Key = 'D',
                        TimeLimit = 8000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 40,
                        Steps = new List<MoveStep>
                        {
                        }
                    },
                    new MoveDef
                    {
                        Name = "Energy Core",
                        Type = "SweetSpot",
                        Key = 'C',
                        TimeLimit = 2000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.01,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        },
        {
            EnemyId.TheNamelessKnight,
            new EnemyDef
            {
                Id = EnemyId.TheNamelessKnight,
                Name = "The Nameless Knight",
                HP = 600,
                MaxHp = 600,
                Attack = 75,
                MinGold = 2000,
                MaxGold = 5000,
                Moves = new List<MoveDef>
                {
                    new MoveDef
                    {
                        Name = "Endless Blade",
                        Type = "MultiStep",
                        Key = 'D',
                        TimeLimit = 2000,
                        Goal = 0,
                        Count = 0,
                        Target = 0.5,
                        Threshold = 0.1,
                        ChainedHitBarPositions = "",
                        PullStrength = 0.001,
                        Steps = new List<MoveStep>
                        {
                            new MoveStep
                            {
                                Type = "Sequence",
                                Key = 'K',
                                Goal = 0,
                                Count = 8,
                                TimeLimit = 2000,
                                FailMsg = "Missed!",
                                ChainedHitBarPositions = "",
                                PullStrength = 0.001
                            },
                            new MoveStep
                            {
                                Type = "Timed",
                                Key = 'X',
                                Goal = 0,
                                Count = 0,
                                TimeLimit = 600,
                                FailMsg = "Missed!",
                                ChainedHitBarPositions = "",
                                PullStrength = 0.001
                            },
                            new MoveStep
                            {
                                Type = "TugOfWar",
                                Key = 'S',
                                Goal = 0,
                                Count = 0,
                                TimeLimit = 5000,
                                FailMsg = "Missed!",
                                ChainedHitBarPositions = "",
                                PullStrength = 35
                            },
                        }
                    }
                }, // End of Moves List
                LootTable = new List<LootItemDef>
                {
                } // End of LootTable List
            }
        }
    };
}
