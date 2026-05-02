using System;
using System.Collections.Generic;

namespace T4Dungeon.Generated;

/// <summary>
/// Auto-generated Enum based on loottables.xml IDs
/// </summary>
public enum LootTableId
{
    EarlyDungeon = 4001
}

public class LootEntry
{
    public ItemId ItemId { get; set; }
    public int Weight { get; set; }
}

public class LootTableDef
{
    public LootTableId Id { get; set; }
    public string Name { get; set; }
    public List<LootEntry> Entries { get; set; } = new();
}

public static class LootDatabase
{
    public static readonly Dictionary<LootTableId, LootTableDef> Tables = new()
    {
        {
            LootTableId.EarlyDungeon,
            new LootTableDef
            {
                Id = LootTableId.EarlyDungeon,
                Name = "Early Dungeon",
                Entries = new List<LootEntry> 
                {
                    new LootEntry { ItemId = (ItemId)1001, Weight = 10 },
                    new LootEntry { ItemId = (ItemId)1002, Weight = 10 },
                    new LootEntry { ItemId = (ItemId)1003, Weight = 10 },
                    new LootEntry { ItemId = (ItemId)1004, Weight = 10 },
                    new LootEntry { ItemId = (ItemId)1005, Weight = 10 },
                    new LootEntry { ItemId = (ItemId)1101, Weight = 10 },
                    new LootEntry { ItemId = (ItemId)1105, Weight = 10 },
                    new LootEntry { ItemId = (ItemId)1201, Weight = 5 },
                    new LootEntry { ItemId = (ItemId)1202, Weight = 5 },
                    new LootEntry { ItemId = (ItemId)2001, Weight = 5 },
                    new LootEntry { ItemId = (ItemId)2002, Weight = 5 },
                    new LootEntry { ItemId = (ItemId)2003, Weight = 5 },
                    new LootEntry { ItemId = (ItemId)2005, Weight = 5 }
                }
            }
        }
    };
}