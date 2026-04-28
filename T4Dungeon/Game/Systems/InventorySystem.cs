using T4Dungeon.Generated;
using T4Dungeon.Game.Models;

namespace T4Dungeon.Game.Systems
{
    public static class InventorySystem
    {
        /// <summary>
        /// Processes the effect of an item and returns the text to be logged.
        /// Now accepts an optional enemy for offensive items.
        /// </summary>
        public static (string Message, bool NeedsMapRedraw) UseItem(Player player, ItemId id, Enemy enemy = null, MapManager map = null)
        {
            bool redraw = false;
            var itemDef = ItemDatabase.Items[id];
            if (!itemDef.IsConsumable) return (string.Empty, false);

            string logMessage = string.Empty;

            foreach (var skillId in itemDef.GrantedSkills)
            {
                var skillDef = SkillDatabase.Skills[skillId];

                // 1. HEALING
                if (skillDef.SkillType == "Healing")
                {
                    int amountToHeal = skillDef.Value;
                    int oldHP = player.HP;
                    player.HP = Math.Min(player.MaxHP, player.HP + amountToHeal);
                    logMessage = $"Used {itemDef.Name}. Restored {player.HP - oldHP} HP!";
                }
                // 2. DAMAGE
                else if (skillDef.SkillType == "Damage")
                {
                    if (enemy != null)
                    {
                        enemy.HP = Math.Max(0, enemy.HP - skillDef.Value);
                        logMessage = $"Used {itemDef.Name}. It deals {skillDef.Value} damage to {enemy.Name}!";
                    }
                    else logMessage = $"Used {itemDef.Name}, but there is no enemy!";
                }
                if (skillDef.Name == "Illuminate" && map != null)
                {
                    map.RevealAdjacent(map.PlayerPosition);
                    logMessage = "The light pushes back the shadows!";
                    redraw = true; // Flag that the map changed
                }
            }

            player.Inventory.Remove(id, 1);
            return (logMessage, redraw);
        }
    }
}