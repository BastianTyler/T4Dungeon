using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4Dungeon.Game.Models;

namespace T4Dungeon.Game.Events
{
    internal class ExitEvent : ICellEvent
    {
        public string Execute(Player player, Cell cell)
        {
            return "Exit";
        }
    }
}
