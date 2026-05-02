using System;
using System.Collections.Generic;
using System.Text;
using T4Dungeon.Game.Models;

namespace T4Dungeon.Game.Events
{
    public class ShopEvent : ICellEvent 
    {
        public bool HasBeenGenerated { get; set; } = false;
        public string Execute(Player player, Cell cell)
        {
            return "Shop";
        }   
    }
}
