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
                Value = 15,
                Cost = 5,
                Description = "A fiery projectile."
            }
        },
        {
            SkillId.Heal,
            new SkillDef
            {
                Id = SkillId.Heal,
                Name = "Heal",
                SkillType = "Healing",
                Value = 10,
                Cost = 5,
                Description = "Restores health."
            }
        },
        {
            SkillId.PommelStrike,
            new SkillDef
            {
                Id = SkillId.PommelStrike,
                Name = "Pommel Strike",
                SkillType = "Damage",
                Value = 8,
                Cost = 3,
                Description = "A quick melee attack."
            }
        }
    };
}