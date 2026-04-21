using System;
using System.Collections.Generic;

namespace T4Dungeon.Generated;

public class SkillDef
{
    public SkillId Id { get; set; }
    public string Name { get; set; }
    public string SkillType { get; set; }
    public int Value { get; set; }
    public int Cost { get; set; }
    public string Description { get; set; }
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
                Cost = 8,
                Description = "A blast of fire."
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
                Cost = 10,
                Description = "Restores health."
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
                Cost = 4,
                Description = "Reduces target defense."
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
                Cost = 5,
                Description = "A wide swinging strike."
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
                Cost = 3,
                Description = "Hits with shield edge."
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
                Cost = 2,
                Description = "Fast but weak shot."
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
                Cost = 15,
                Description = "High energy strike."
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
                Cost = 0,
                Description = "Restores mana."
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
                Cost = 5,
                Description = "Powerful blow, hurts self."
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
                Cost = 20,
                Description = "Massive restoration."
            }
        }
    };
}