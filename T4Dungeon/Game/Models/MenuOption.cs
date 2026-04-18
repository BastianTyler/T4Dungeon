using System;
using System.Collections.Generic;
using System.Text;

namespace T4Dungeon.Game.Models
{
    public class MenuOption
    {
        public string Text { get; set; }
        public Action? Action { get; set; }
        public bool IsImplemented { get; set; } = true;
    }
}
