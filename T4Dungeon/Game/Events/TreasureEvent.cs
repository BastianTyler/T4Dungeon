using System;
using System.Collections.Generic;
using System.Text;
using T4Dungeon.Game.Models;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Events
{
    public class TreasureEvent : ICellEvent
    {
        private static Random _random = new Random();
        public string Execute(Player player, Cell cell)
        {
            // 1. Get all possible Item IDs 
            List<ItemId> allItemIds = ItemDatabase.Items.Keys.ToList();

            // 2. Pick a random one
            ItemId droppedId = allItemIds[_random.Next(allItemIds.Count)];
            ItemDef droppedItem = ItemDatabase.Items[droppedId];

            //3. Add it to the player's inventory
            player.Inventory.Add(droppedId);

            return $"You found a treasure! Inside was a {droppedItem.Name}. {droppedItem.Description}";
        }
    }
}
