using System;
using System.Collections.Generic;
using System.Text;
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
        }
    }
}
