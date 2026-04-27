using System;
using System.Collections.Generic;

namespace T4Dungeon.Generated;

public class SkillDef
{
    public SkillId Id { get; set; }
    public string Name { get; set; }
    public string SkillType { get; set; }
    public int Value { get; set; }
    public string Description { get; set; }
    public List<SkillStep> Steps { get; set; } = new();
    public List<SkillCost> ResourceCosts { get; set; } = new();
}

public class SkillStep
{
    public string Type { get; set; }
    public char Key { get; set; }
    public int Goal { get; set; }
    public int Time { get; set; }
    public string FailMsg { get; set; }
    public string ChainedHitBarPositions { get; set; }
}

public class SkillCost
{
    public string ResourceType { get; set; }
    public int Amount { get; set; }
}

public static class SkillDatabase
{
    public static readonly Dictionary<SkillId, SkillDef> Skills = new()
    {
        {
            SkillId.Fireball,
            new SkillDef
            {
                Id = SkillId.Fireball,
                Name = "Fireball",
                SkillType = "Damage",
                Value = 20,
                Description = "A blast of fire.",
                ResourceCosts = new List<SkillCost>
                {
                    new SkillCost { ResourceType = "Mana", Amount = 8 },
                },
                Steps = new List<SkillStep> 
                {
                    new SkillStep { Type = "Timed", Key = 'F', Goal = 1, Time = 1200, FailMsg = "The flames sputtered out!", ChainedHitBarPositions = "" },
                }
            }
        },
        {
            SkillId.Heal,
            new SkillDef
            {
                Id = SkillId.Heal,
                Name = "Heal",
                SkillType = "Healing",
                Value = 25,
                Description = "Restores health.",
                ResourceCosts = new List<SkillCost>
                {
                    new SkillCost { ResourceType = "Mana", Amount = 10 },
                },
                Steps = new List<SkillStep> 
                {
                }
            }
        },
        {
            SkillId.Sunder,
            new SkillDef
            {
                Id = SkillId.Sunder,
                Name = "Sunder",
                SkillType = "Damage",
                Value = 12,
                Description = "Reduces target defense.",
                ResourceCosts = new List<SkillCost>
                {
                    new SkillCost { ResourceType = "Mana", Amount = 4 },
                },
                Steps = new List<SkillStep> 
                {
                }
            }
        },
        {
            SkillId.Cleave,
            new SkillDef
            {
                Id = SkillId.Cleave,
                Name = "Cleave",
                SkillType = "Damage",
                Value = 15,
                Description = "A wide swinging strike.",
                ResourceCosts = new List<SkillCost>
                {
                    new SkillCost { ResourceType = "Stamina", Amount = 25 },
                },
                Steps = new List<SkillStep> 
                {
                    new SkillStep { Type = "ChainedHitBar", Key = ' ', Goal = 1, Time = 3000, FailMsg = "Momentum lost!", ChainedHitBarPositions = "0.25, 0.5, 0.75" },
                }
            }
        },
        {
            SkillId.HeavyCleave,
            new SkillDef
            {
                Id = SkillId.HeavyCleave,
                Name = "Heavy Cleave",
                SkillType = "Damage",
                Value = 35,
                Description = "A brutal two-stage overhead strike.",
                ResourceCosts = new List<SkillCost>
                {
                    new SkillCost { ResourceType = "Mana", Amount = 12 },
                },
                Steps = new List<SkillStep> 
                {
                    new SkillStep { Type = "Mash", Key = 'H', Goal = 20, Time = 2500, FailMsg = "The weight of the blade was too much!", ChainedHitBarPositions = "" },
                    new SkillStep { Type = "Timed", Key = 'X', Goal = 1, Time = 600, FailMsg = "You swung wide and hit the dirt!", ChainedHitBarPositions = "" },
                }
            }
        },
        {
            SkillId.ShieldBash,
            new SkillDef
            {
                Id = SkillId.ShieldBash,
                Name = "Shield Bash",
                SkillType = "Damage",
                Value = 8,
                Description = "Hits with shield edge.",
                ResourceCosts = new List<SkillCost>
                {
                    new SkillCost { ResourceType = "Mana", Amount = 3 },
                },
                Steps = new List<SkillStep> 
                {
                }
            }
        },
        {
            SkillId.QuickShot,
            new SkillDef
            {
                Id = SkillId.QuickShot,
                Name = "Quick Shot",
                SkillType = "Damage",
                Value = 10,
                Description = "Fast but weak shot.",
                ResourceCosts = new List<SkillCost>
                {
                    new SkillCost { ResourceType = "Mana", Amount = 2 },
                },
                Steps = new List<SkillStep> 
                {
                    new SkillStep { Type = "Timed", Key = 'Q', Goal = 1, Time = 500, FailMsg = "Slow on the draw!", ChainedHitBarPositions = "" },
                }
            }
        },
        {
            SkillId.LightningBolt,
            new SkillDef
            {
                Id = SkillId.LightningBolt,
                Name = "Lightning Bolt",
                SkillType = "Damage",
                Value = 35,
                Description = "High energy strike.",
                ResourceCosts = new List<SkillCost>
                {
                    new SkillCost { ResourceType = "Mana", Amount = 15 },
                },
                Steps = new List<SkillStep> 
                {
                }
            }
        },
        {
            SkillId.Meditate,
            new SkillDef
            {
                Id = SkillId.Meditate,
                Name = "Meditate",
                SkillType = "Mana",
                Value = 15,
                Description = "Restores mana.",
                ResourceCosts = new List<SkillCost>
                {
                    new SkillCost { ResourceType = "Mana", Amount = 0 },
                },
                Steps = new List<SkillStep> 
                {
                }
            }
        },
        {
            SkillId.Bloodlust,
            new SkillDef
            {
                Id = SkillId.Bloodlust,
                Name = "Bloodlust",
                SkillType = "Damage",
                Value = 25,
                Description = "Powerful blow, hurts self.",
                ResourceCosts = new List<SkillCost>
                {
                    new SkillCost { ResourceType = "Mana", Amount = 5 },
                },
                Steps = new List<SkillStep> 
                {
                }
            }
        },
        {
            SkillId.HolyLight,
            new SkillDef
            {
                Id = SkillId.HolyLight,
                Name = "Holy Light",
                SkillType = "Healing",
                Value = 50,
                Description = "Massive restoration.",
                ResourceCosts = new List<SkillCost>
                {
                    new SkillCost { ResourceType = "Mana", Amount = 20 },
                },
                Steps = new List<SkillStep> 
                {
                }
            }
        }
    };
}