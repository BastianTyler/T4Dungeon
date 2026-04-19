using System;
using System.Collections.Generic;
using System.Text;
using T4Dungeon.Game.Models;

namespace T4Dungeon.Game.Events
{
    public class TreasureEvent : ICellEvent
    {
        public string Execute(Player player, Cell cell)
        {
            return "You found a treasure!";
        }
    }
}
