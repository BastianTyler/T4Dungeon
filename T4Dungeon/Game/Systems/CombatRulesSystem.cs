using T4Dungeon.Game.Models;

namespace T4Dungeon.Game.Systems
{
    public class CombatRulesSystem
    {
        public int CalculateAttack(Player player, Enemy enemy)
        {
            return player.Attack;
        }

        public void ApplyDefense(Player player, int defenceBonus)
        {
            player.IsDefending = true;
            player.BaseDefense += defenceBonus;
        }
        public void RemoveDefence(Player player, int defenceBonus)
        {
            player.IsDefending = true;
            player.BaseDefense -= defenceBonus;
        }

        public bool RollFlee(Player player, Enemy enemy)
        {
            return Random.Shared.Next(100) < 50;
        }

        public void ApplyEnemyDamage(Player player, Enemy enemy)
        {
            int damage = enemy.Attack;

            if (player.IsDefending)
                damage = Math.Max(0, damage - player.BaseDefense);

            player.HP -= damage;
        }
    }
}