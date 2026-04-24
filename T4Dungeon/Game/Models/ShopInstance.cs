using System;
using System.Collections.Generic;
using System.Linq;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Models
{
    public class ShopSlot
    {
        public ItemId ItemId { get; set; }
        public int Price { get; set; }
        public bool IsSold { get; set; }
        public bool IsDiscounted { get; set; }

        public ShopSlot(ItemDef def)
        {
            ItemId = def.Id;
            Price = def.BasePrice;
            IsSold = false;
            IsDiscounted = false;
        }

        public class ShopInstance
        {
            public List<ShopSlot> Inventory { get; set; } = new();

            /// <summary>
            /// Logic moved from GameEngine. Handles stock generation.
            /// </summary>
            public void GenerateInventory()
            {
                Random rng = new Random();
                var allItems = ItemDatabase.Items.Values.ToList();

                // 1. Guaranteed Healing Item
                var heal = allItems.FirstOrDefault(i => i.Category == "Healing") ?? allItems[0];
                Inventory.Add(new ShopSlot(heal));

                // 2. Guaranteed Buff Item
                var buff = allItems.FirstOrDefault(i => i.Category == "Buff") ?? allItems[1];
                Inventory.Add(new ShopSlot(buff));

                // 3. Fill randoms
                while (Inventory.Count < 8)
                {
                    var rand = allItems[rng.Next(allItems.Count)];
                    if (!Inventory.Any(s => s.ItemId == rand.Id))
                        Inventory.Add(new ShopSlot(rand));
                }

                // 4. Apply Discount
                var discountSlot = Inventory[rng.Next(Inventory.Count)];
                discountSlot.Price = (int)(discountSlot.Price * 0.1);
                discountSlot.IsDiscounted = true;
            }

            /// <summary>
            /// Logic moved from GameEngine. Validates if a purchase can happen.
            /// </summary>
            public bool PurchaseItem(ShopSlot slot, Player player)
            {
                if (player.Gold >= slot.Price && !slot.IsSold)
                {
                    player.Gold -= slot.Price;
                    player.Inventory.Add(slot.ItemId, 1);
                    slot.IsSold = true;
                    return true;
                }
                return false;
            }
        }
    }
}