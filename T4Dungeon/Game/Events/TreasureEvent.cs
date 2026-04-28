using System;
using System.Collections.Generic;
using System.Linq;
using T4Dungeon.Game.Models;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Events
{
    public class TreasureEvent : ICellEvent
    {
        private static Random _random = new Random();
        private readonly LootTableId _tableId;

        // Default to EarlyDungeon so your existing map generation doesn't break
        public TreasureEvent(LootTableId tableId = LootTableId.EarlyDungeon)
        {
            _tableId = tableId;
        }

        public string Execute(Player player, Cell cell)
        {
            // 1. Get the table definition from the generated database
            if (!LootDatabase.Tables.TryGetValue(_tableId, out var table))
            {
                return "You found a chest, but it was empty...";
            }

            // 2. Perform the weighted roll
            int totalWeight = table.Entries.Sum(e => e.Weight);
            int roll = _random.Next(totalWeight);
            int cumulativeWeight = 0;

            ItemId droppedId = table.Entries.First().ItemId; // Fallback

            foreach (var entry in table.Entries)
            {
                cumulativeWeight += entry.Weight;
                if (roll < cumulativeWeight)
                {
                    droppedId = entry.ItemId;
                    break;
                }
            }

            // 3. Get item data for the message
            ItemDef droppedItem = ItemDatabase.Items[droppedId];

            // 4. Add exactly 1 to inventory (no amount logic needed)
            player.Inventory.Add(droppedId);

            return $"You found a treasure! Inside was a {droppedItem.Name}. {droppedItem.Description}";
        }
    }
}