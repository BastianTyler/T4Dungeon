using System;
using System.Collections.Generic;
using System.Text;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Models
{
    public class Inventory
    {

        public readonly List<InventoryItem> _items = new();
        public IReadOnlyList<InventoryItem> Items => _items;

        public void Add(ItemId id, int amount = 1)
        {
            var existingItem = _items.FirstOrDefault(i => i.ItemId == id);

            if (existingItem != null)
                existingItem.Amount += amount;
            else
                _items.Add(new InventoryItem { ItemId = id, Amount = amount });
        }

        public void Remove(ItemId id, int amount = 1)
        {
            // Find the item entry in your internal list/dictionary
            var existing = _items.FirstOrDefault(i => i.ItemId == id);

            if (existing != null)
            {
                existing.Amount -= amount;

                // If we ran out of the item, wipe it from the list
                if (existing.Amount <= 0)
                {
                    _items.Remove(existing);
                }
            }
        }
    }
}
