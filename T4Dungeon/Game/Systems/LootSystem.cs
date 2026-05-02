using T4Dungeon.Game.Models;
using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Systems
{
    public class LootSystem
    {
        private readonly GameLogSystem _log;
        private readonly Random _rng = new();

        public LootSystem(GameLogSystem log)
        {
            _log = log;
        }

        public void ProcessLoot(Player player, Enemy enemy)
        {
            var def = EnemyDatabase.Enemies.Values.First(e => e.Name == enemy.Name);

            int goldDropped = _rng.Next(def.MinGold, def.MaxGold + 1);
            player.Gold += goldDropped;
            _log.Add($"{TextColor.Yellow}The {enemy.Name} dropped {goldDropped} gold!{TextColor.Reset}");

            foreach (var loot in def.LootTable)
            {
                if (_rng.NextDouble() <= loot.Chance)
                {
                    player.Inventory.Add(loot.Id, 1);
                    var itemDef = ItemDatabase.Items[loot.Id];
                    _log.Add($"{TextColor.Cyan}LOOTED: {itemDef.Name}!{TextColor.Reset}");
                }
            }
        }
    }
}