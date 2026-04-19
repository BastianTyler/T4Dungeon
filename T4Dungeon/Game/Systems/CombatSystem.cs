using System;
using System.Collections.Generic;
using System.Text;
using T4Dungeon.Game.Models;

namespace T4Dungeon.Game.Systems
{
    public class CombatSystem
    {
        private Player _player;
        private Enemy _enemy;

        private bool _combatOver;
        private string _message;

        public bool IsOver => _combatOver;
        public string Message => _message;
        public Enemy Enemy => _enemy;

        public CombatSystem(Player player, Enemy enemy)
        {
            _player = player;
            _enemy = enemy;
            _combatOver = false;
            _message = $"You encountered a {_enemy.Name}!";
        }

        public void RunTurn(Action playerAction)
        {
            if (_combatOver) return;

            playerAction?.Invoke();

            if (CheckEnd()) return;

            EnemyTurn();

            CheckEnd();
        }

        public bool CheckEnd()
        { 
            if(_player.HP <= 0)
            {
                _combatOver = true;
                _message = "You died.";
                return true;
            }

            if(_enemy.HP <= 0)
            {
                _combatOver = true;
                _message = $"You defeated the {_enemy.Name}!";
                return true;
            }
            return false;
        }

        private bool TimedInput(char expectedKey, int timeLimitSeconds = 2000)
        {
            var start = DateTime.Now;

            while ((DateTime.Now - start).TotalMilliseconds < timeLimitSeconds)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).KeyChar;
                    return key == expectedKey;
                }
            }
            return false;
        }

        private void EnemyTurn()
        {
            _message = "Enemy is attacking! Press D to defend!";

            bool defended = TimedInput('d');

            if (defended)
            {
                _message = "You blocked the attack!";
                return;
            }

            _player.HP -= _enemy.Attack;
            _message = "You took damage!";
        }

        public void Attack()
        {
            _enemy.HP -= _player.Attack;
        }

        public void Defend()
        {
            _player.IsDefending = true;
        }

        public bool TryFlee()
        {
            return new Random().Next(100) < 50;
        }

    }
}
