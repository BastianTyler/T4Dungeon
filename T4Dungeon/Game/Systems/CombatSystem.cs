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
        private Action<string, bool> _logger;
        private void _log(string m, bool w = true) => _logger?.Invoke(m, w);

        public CombatSystem(Player player, Enemy enemy, Action<string, bool> logger)
        {
            _player = player;
            _enemy = enemy;
            _combatOver = false;
            _logger = logger;
            _log($"You encountered a {_enemy.Name}!", false);
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
                _log("You died.");
                return true;
            }

            if(_enemy.HP <= 0)
            {
                _combatOver = true;
                _log($"You defeated the {_enemy.Name}!");
                return true;
            }
            return false;
        }

        private bool TimedInput(char expectedKey, int timeLimitMs = 2000)
        {
            while (Console.KeyAvailable) Console.ReadKey(intercept: true);

            var start = DateTime.Now;
            int totalBarLength = 30;

            // Save the current cursor position to restore it later
            int cursorTop = Console.CursorTop;

            while ((DateTime.Now - start).TotalMilliseconds < timeLimitMs)
            {
                double elapsed = (DateTime.Now - start).TotalMilliseconds;
                double percent = elapsed / timeLimitMs;

                int barsToDraw = (int)(percent * totalBarLength);
                string bar = new string('|', barsToDraw).PadRight(totalBarLength, '.');

                // DIRECT CONSOLE MANIPULATION (No flicker)
                // We target the area just below the log or at a fixed bottom position
                Console.SetCursorPosition(0, cursorTop);
                Console.Write($"[ {bar} ] - PRESS {char.ToUpper(expectedKey)}!    ");

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).KeyChar;
                    // Clean up the bar line before returning
                    ClearLine(cursorTop);
                    return char.ToLower(key) == char.ToLower(expectedKey);
                }

                Thread.Sleep(20); // Smooth 50fps update
            }

            ClearLine(cursorTop);
            return false;
        }

        private void ClearLine(int row)
        {
            Console.SetCursorPosition(0, row);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, row);
        }

        private bool MashInput(char keyToMash, int goalStrikes, int timeLimitMs = 3000)
        {
            while (Console.KeyAvailable) Console.ReadKey(true);

            var start = DateTime.Now;
            int currentStrikes = 0;
            int totalBarLength = 30;
            int cursorTop = Console.CursorTop;

            while ((DateTime.Now - start).TotalMilliseconds < timeLimitMs)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).KeyChar;
                    if (char.ToLower(key) == char.ToLower(keyToMash))
                        currentStrikes++;
                }

                if (currentStrikes >= goalStrikes)
                {
                    ClearLine(cursorTop);
                    return true; // Success!
                }

                // Draw progress bar based on strikes vs goal
                double percent = (double)currentStrikes / goalStrikes;
                int barsToDraw = (int)(percent * totalBarLength);
                string bar = new string('#', barsToDraw).PadRight(totalBarLength, '-');

                Console.SetCursorPosition(0, cursorTop);
                Console.Write($"[ {bar} ] MASH {char.ToUpper(keyToMash)}! ({currentStrikes}/{goalStrikes})   ");

                Thread.Sleep(20);
            }

            ClearLine(cursorTop);
            return false; // Time's up
        }

        private bool SweetSpotInput(char stopKey, double targetPercent = 0.5, double threshold = 0.1, int speedMs = 1500)
        {
            while (Console.KeyAvailable) Console.ReadKey(true);

            var start = DateTime.Now;
            int totalBarLength = 30;
            int cursorTop = Console.CursorTop;
            int targetIndex = (int)(targetPercent * totalBarLength);

            while (true)
            {
                // Use Trig to make the bar bounce back and forth
                double elapsed = (DateTime.Now - start).TotalMilliseconds;
                double progress = (Math.Sin(elapsed / speedMs * Math.PI * 2) + 1) / 2; // Returns 0 to 1

                int markerIndex = (int)(progress * totalBarLength);

                // Build the visual bar
                char[] bar = new string('-', totalBarLength).ToCharArray();
                bar[targetIndex] = 'V'; // The goal

                // Draw the cursor
                Console.SetCursorPosition(0, cursorTop);
                string visualBar = new string(bar);
                // We replace the char at markerIndex with a highlight for the player
                Console.Write($"[ {visualBar.Remove(markerIndex, 1).Insert(markerIndex, "X")} ] - STOP AT V! ");

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).KeyChar;
                    if (char.ToLower(key) == char.ToLower(stopKey))
                    {
                        double distance = Math.Abs(progress - targetPercent);
                        ClearLine(cursorTop);
                        return distance <= threshold;
                    }
                }

                Thread.Sleep(15);
            }
        }

        private void EnemyTurn()
        {
            int moveType = new Random().Next(3);

            if (moveType == 0)
            {
                _log("Quick attack! React!", false);
                if (TimedInput('d')) _log("Parried!");
                else TakeDamage();
            }
            else if (moveType == 1)
            {
                _log("The enemy is crushing you! MASH SPACE!", false);
                if (MashInput(' ', 15)) _log("You pushed them back!");
                else TakeDamage();
            }
            else
            {
                _log("Powerful swing incoming! Time your block!", false);
                if (SweetSpotInput('d', 0.8, 0.08)) _log("Perfect block!");
                else TakeDamage();
            }
        }

        private void TakeDamage()
        {
            _player.HP -= _enemy.Attack;
            _log("You took damage!");
        }

        public void Attack()
        {
            _enemy.HP -= _player.Attack;
            _log($"You attacked the {_enemy.Name} for {_player.Attack} damage!");
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
