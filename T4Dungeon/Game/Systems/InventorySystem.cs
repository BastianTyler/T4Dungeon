using T4Dungeon.Generated;
using T4Dungeon.Game.Models;

namespace T4Dungeon.Game.Systems
{
    public static class InventorySystem
    {
        /// <summary>
        /// Processes the effect of an item and returns the text to be logged.
        /// </summary>
        public static string UseItem(Player player, ItemId id)
        {
            var itemDef = ItemDatabase.Items[id];
            if (!itemDef.IsConsumable) return string.Empty;

            string logMessage = string.Empty;

            foreach (var skillId in itemDef.GrantedSkills)
            {
                var skillDef = SkillDatabase.Skills[skillId];

                if (skillDef.SkillType == "Healing")
                {
                    int amountToHeal = skillDef.Value;
                    int oldHP = player.HP;
                    player.HP = Math.Min(player.MaxHP, player.HP + amountToHeal);
                    int actualHeal = player.HP - oldHP;

                    logMessage = $"Used {itemDef.Name}. Restored {actualHeal} HP!";
                }
            }

            player.Inventory.Remove(id, 1);
            return logMessage;
        }
    }
}