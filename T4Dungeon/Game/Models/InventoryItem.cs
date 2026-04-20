using System;
using System.Collections.Generic;
using System.Text;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Models
{
    public class InventoryItem
    {
        public ItemId ItemId { get; set;  }
        public int Amount { get; set; } 
    }
}
