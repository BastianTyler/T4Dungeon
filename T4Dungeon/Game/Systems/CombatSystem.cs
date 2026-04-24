using System;
using System.Collections.Generic;
using System.Text;
using T4Dungeon.Game.Models;
using T4Dungeon.Generated;

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
                ProcessLoot();
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

            // Calculate integer indices for the visual zone
            int targetIndex = (int)(targetPercent * totalBarLength);
            int halfZoneWidth = (int)(threshold * totalBarLength);

            // Ensure the zone is at least 1 character wide so it's never impossible
            if (halfZoneWidth < 1) halfZoneWidth = 1;

            int zoneStart = Math.Max(0, targetIndex - halfZoneWidth);
            int zoneEnd = Math.Min(totalBarLength - 1, targetIndex + halfZoneWidth);

            while (true)
            {
                double elapsed = (DateTime.Now - start).TotalMilliseconds;
                // Periodic bounce 0 to 1 to 0
                double progress = (Math.Sin(elapsed / speedMs * Math.PI * 2) + 1) / 2;
                int markerIndex = (int)(progress * totalBarLength);

                // --- RENDER LOGIC ---
                Console.SetCursorPosition(0, cursorTop);
                Console.Write("[ ");
                for (int i = 0; i < totalBarLength; i++)
                {
                    if (i == markerIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("X");
                    }
                    else if (i == targetIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("V");
                    }
                    else if (i >= zoneStart && i <= zoneEnd)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("=");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("-");
                    }
                    Console.ResetColor();
                }
                Console.Write(" ] - SPACE TO HIT!   ");

                // --- INPUT LOGIC ---
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).KeyChar;
                    if (char.ToLower(key) == char.ToLower(stopKey))
                    {
                        // WIN CONDITION: Is the marker index inside the green zone index?
                        bool isVisualHit = markerIndex >= zoneStart && markerIndex <= zoneEnd;

                        ClearLine(cursorTop);
                        return isVisualHit;
                    }
                }

                Thread.Sleep(15);
            }
        }

        public void EnemyTurn()
        {
            
            var move = _enemy.Moves[new Random().Next(_enemy.Moves.Count)];

            _log($"{_enemy.Name} uses {move.Name}! PRESS {move.Key}!", false);

            bool success = false;

            
            switch (move.Type)
            {
                case "Timed":
                    success = TimedInput(move.Key, move.TimeLimit);
                    break;
                case "Mash":
                    success = MashInput(move.Key, move.Goal, move.TimeLimit);
                    break;
                case "SweetSpot":
                    success = SweetSpotInput(move.Key, move.Target, move.Threshold);
                    break;
                case "Sequence":
                    success = SequenceInput(move.Key, move.Count);
                    break;
            }

            if (success)
            {
                _log($"Successfully countered {move.Name}!");
            }
            else
            {
                _player.TakeDamage(_enemy.Attack);
                _log($"Failed! You took damage!");
            }
        }

        private void ProcessLoot()
        {
            Random rng = new Random();

            // 1. Calculate Gold
            // We fetch these values from the enemy definition
            var def = EnemyDatabase.Enemies.Values.First(e => e.Name == _enemy.Name);
            int goldDropped = rng.Next(def.MinGold, def.MaxGold + 1);

            _player.Gold += goldDropped;
            _log($"The {_enemy.Name} dropped {goldDropped} gold!", true);

            // 2. Roll for Items
            foreach (var loot in def.LootTable)
            {
                if (rng.NextDouble() <= loot.Chance)
                {
                    _player.Inventory.Add(loot.Id, 1);
                    var itemDef = ItemDatabase.Items[loot.Id];
                    _log($"LOOTED: {itemDef.Name}!", true);
                }
            }
        }

        private bool SequenceInput(char key, int count, int timeLimitPerPress = 1000)
        {
            for (int i = 0; i < count; i++)
            {
                _log($"({i + 1}/{count}) QUICK! PRESS {char.ToUpper(key)}!", false);

                if (!TimedInput(key, timeLimitPerPress))
                {
                    return false;
                }

                // Small buffer so one key press doesn't count for two
                System.Threading.Thread.Sleep(100);
            }
            return true;
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
