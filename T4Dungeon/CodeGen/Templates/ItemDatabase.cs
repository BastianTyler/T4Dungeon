using System.Collections.Generic;

namespace T4Dungeon.Generated;

public class ItemDef
{
    public ItemId Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public static class ItemDatabase
{
    public static readonly Dictionary<ItemId, ItemDef> Items = new()
    {
        {
            ItemId.IronSword,
            new ItemDef
            {
                Id = ItemId.IronSword,
                Name = "Iron Sword",
                Description = "No description available."
            }
        },
        {
            ItemId.WoodenShield,
            new ItemDef
            {
                Id = ItemId.WoodenShield,
                Name = "Wooden Shield",
                Description = "No description available."
            }
        },
        {
            ItemId.ApprenticeRing,
            new ItemDef
            {
                Id = ItemId.ApprenticeRing,
                Name = "Apprentice Ring",
                Description = "No description available."
            }
        }
    };
}