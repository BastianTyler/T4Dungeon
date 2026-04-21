using System.Collections.Generic;

namespace T4Dungeon.Generated;

public enum EquiptSlot { None, Weapon, Armor, Accessory }

public class ItemDef
{
    public ItemId Id { get; set; }
    public string Name { get; set; }
    public EquiptSlot Slot { get; set; }
    public string Description { get; set; }
    public int AttackBonus { get; set; }
    public int DefenseBonus { get; set; }
    public List<SkillId> GrantedSkills { get; set; } = new();
    public bool IsStackable { get; set; }
    public bool IsConsumable { get; set; }
}

public static class ItemDatabase
{
    public static readonly Dictionary<ItemId, ItemDef> Items = new()
    {
        {
            ItemId.RustyDagger,
            new ItemDef
            {
                Id = ItemId.RustyDagger,
                Name = "Rusty Dagger",
                Slot = EquiptSlot.Weapon,
                Description = "Better than nothing.",
                AttackBonus = 2,
                DefenseBonus = 0,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> {  }
            }
        },
        {
            ItemId.IronSword,
            new ItemDef
            {
                Id = ItemId.IronSword,
                Name = "Iron Sword",
                Slot = EquiptSlot.Weapon,
                Description = "Reliable steel.",
                AttackBonus = 5,
                DefenseBonus = 0,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> {  }
            }
        },
        {
            ItemId.Warhammer,
            new ItemDef
            {
                Id = ItemId.Warhammer,
                Name = "Warhammer",
                Slot = EquiptSlot.Weapon,
                Description = "Heavy and brutal.",
                AttackBonus = 9,
                DefenseBonus = 0,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> { SkillId.Cleave }
            }
        },
        {
            ItemId.WizardStaff,
            new ItemDef
            {
                Id = ItemId.WizardStaff,
                Name = "Wizard Staff",
                Slot = EquiptSlot.Weapon,
                Description = "Channels arcane energy.",
                AttackBonus = 3,
                DefenseBonus = 0,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> { SkillId.Fireball }
            }
        },
        {
            ItemId.Longbow,
            new ItemDef
            {
                Id = ItemId.Longbow,
                Name = "Longbow",
                Slot = EquiptSlot.Weapon,
                Description = "Shoot from afar.",
                AttackBonus = 6,
                DefenseBonus = 0,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> { SkillId.QuickShot }
            }
        },
        {
            ItemId.Excalibur,
            new ItemDef
            {
                Id = ItemId.Excalibur,
                Name = "Excalibur",
                Slot = EquiptSlot.Weapon,
                Description = "The legendary blade.",
                AttackBonus = 25,
                DefenseBonus = 0,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> { SkillId.HolyLight }
            }
        },
        {
            ItemId.ClothTunic,
            new ItemDef
            {
                Id = ItemId.ClothTunic,
                Name = "Cloth Tunic",
                Slot = EquiptSlot.Armor,
                Description = "Very breezy.",
                AttackBonus = 0,
                DefenseBonus = 1,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> {  }
            }
        },
        {
            ItemId.LeatherArmor,
            new ItemDef
            {
                Id = ItemId.LeatherArmor,
                Name = "Leather Armor",
                Slot = EquiptSlot.Armor,
                Description = "Light and flexible.",
                AttackBonus = 0,
                DefenseBonus = 4,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> {  }
            }
        },
        {
            ItemId.Chainmail,
            new ItemDef
            {
                Id = ItemId.Chainmail,
                Name = "Chainmail",
                Slot = EquiptSlot.Armor,
                Description = "Solid protection.",
                AttackBonus = 0,
                DefenseBonus = 8,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> {  }
            }
        },
        {
            ItemId.PlateArmor,
            new ItemDef
            {
                Id = ItemId.PlateArmor,
                Name = "Plate Armor",
                Slot = EquiptSlot.Armor,
                Description = "A walking fortress.",
                AttackBonus = 0,
                DefenseBonus = 15,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> {  }
            }
        },
        {
            ItemId.WoodenShield,
            new ItemDef
            {
                Id = ItemId.WoodenShield,
                Name = "Wooden Shield",
                Slot = EquiptSlot.Armor,
                Description = "A sturdy plank.",
                AttackBonus = 0,
                DefenseBonus = 3,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> { SkillId.ShieldBash }
            }
        },
        {
            ItemId.DragonScale,
            new ItemDef
            {
                Id = ItemId.DragonScale,
                Name = "Dragon Scale",
                Slot = EquiptSlot.Armor,
                Description = "Impenetrable hide.",
                AttackBonus = 0,
                DefenseBonus = 22,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> {  }
            }
        },
        {
            ItemId.OldPenny,
            new ItemDef
            {
                Id = ItemId.OldPenny,
                Name = "Old Penny",
                Slot = EquiptSlot.Accessory,
                Description = "Might be lucky.",
                AttackBonus = 0,
                DefenseBonus = 1,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> {  }
            }
        },
        {
            ItemId.ApprenticeRing,
            new ItemDef
            {
                Id = ItemId.ApprenticeRing,
                Name = "Apprentice Ring",
                Slot = EquiptSlot.Accessory,
                Description = "A student's trinket.",
                AttackBonus = 1,
                DefenseBonus = 0,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> { SkillId.Meditate }
            }
        },
        {
            ItemId.IronBand,
            new ItemDef
            {
                Id = ItemId.IronBand,
                Name = "Iron Band",
                Slot = EquiptSlot.Accessory,
                Description = "Increases grip strength.",
                AttackBonus = 2,
                DefenseBonus = 0,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> {  }
            }
        },
        {
            ItemId.AmuletofLife,
            new ItemDef
            {
                Id = ItemId.AmuletofLife,
                Name = "Amulet of Life",
                Slot = EquiptSlot.Accessory,
                Description = "Faintly glowing.",
                AttackBonus = 0,
                DefenseBonus = 2,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> { SkillId.Heal }
            }
        },
        {
            ItemId.ChaosOrb,
            new ItemDef
            {
                Id = ItemId.ChaosOrb,
                Name = "Chaos Orb",
                Slot = EquiptSlot.Accessory,
                Description = "It whispers to you.",
                AttackBonus = 10,
                DefenseBonus = 0,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> { SkillId.Bloodlust }
            }
        },
        {
            ItemId.StormPendant,
            new ItemDef
            {
                Id = ItemId.StormPendant,
                Name = "Storm Pendant",
                Slot = EquiptSlot.Accessory,
                Description = "Crackles with static.",
                AttackBonus = 5,
                DefenseBonus = 0,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> { SkillId.LightningBolt }
            }
        },
        {
            ItemId.ShieldRing,
            new ItemDef
            {
                Id = ItemId.ShieldRing,
                Name = "Shield Ring",
                Slot = EquiptSlot.Accessory,
                Description = "Provides a tiny barrier.",
                AttackBonus = 0,
                DefenseBonus = 5,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> {  }
            }
        },
        {
            ItemId.BerserkerCharm,
            new ItemDef
            {
                Id = ItemId.BerserkerCharm,
                Name = "Berserker Charm",
                Slot = EquiptSlot.Accessory,
                Description = "Reduces defense for power.",
                AttackBonus = 15,
                DefenseBonus = -5,
                IsStackable = false,
                IsConsumable = false,
                GrantedSkills = new List<SkillId> {  }
            }
        },
        {
            ItemId.HealthPotion,
            new ItemDef
            {
                Id = ItemId.HealthPotion,
                Name = "Health Potion",
                Slot = EquiptSlot.None,
                Description = "Tastes like cherries and magic.",
                AttackBonus = 0,
                DefenseBonus = 0,
                IsStackable = true,
                IsConsumable = true,
                GrantedSkills = new List<SkillId> { SkillId.Heal }
            }
        },
        {
            ItemId.ManaPotion,
            new ItemDef
            {
                Id = ItemId.ManaPotion,
                Name = "Mana Potion",
                Slot = EquiptSlot.None,
                Description = "A shimmering blue liquid.",
                AttackBonus = 0,
                DefenseBonus = 0,
                IsStackable = true,
                IsConsumable = true,
                GrantedSkills = new List<SkillId> { SkillId.Meditate }
            }
        },
        {
            ItemId.FireScroll,
            new ItemDef
            {
                Id = ItemId.FireScroll,
                Name = "Fire Scroll",
                Slot = EquiptSlot.None,
                Description = "Single-use pyrotechnics.",
                AttackBonus = 0,
                DefenseBonus = 0,
                IsStackable = true,
                IsConsumable = true,
                GrantedSkills = new List<SkillId> { SkillId.Fireball }
            }
        },
        {
            ItemId.IronSkinElixir,
            new ItemDef
            {
                Id = ItemId.IronSkinElixir,
                Name = "Iron Skin Elixir",
                Slot = EquiptSlot.None,
                Description = "Hardens the skin temporarily.",
                AttackBonus = 0,
                DefenseBonus = 0,
                IsStackable = true,
                IsConsumable = true,
                GrantedSkills = new List<SkillId> { SkillId.ShieldBash }
            }
        }
    };
}