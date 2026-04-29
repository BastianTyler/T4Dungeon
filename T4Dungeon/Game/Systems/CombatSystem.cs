using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using T4Dungeon.Game.Core;
using T4Dungeon.Game.Models;
using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Systems
{
    public class CombatSystem
    {
        private Player _player;
        private Enemy _enemy;
        private TutorialManager _tutorial;

        private bool _combatOver;
        private string _message;

        public bool IsOver => _combatOver;
        public string Message => _message;
        public Enemy Enemy => _enemy;
        private Action<string, bool> _logger;
        private void _log(string m, bool w = true) => _logger?.Invoke(m, w);

        public CombatSystem(Player player, Enemy enemy, TutorialManager tutorial, Action<string, bool> logger)
        {
            _player = player;
            _enemy = enemy;
            _tutorial = tutorial;
            _combatOver = false;
            _logger = logger;
            _log($"You encountered a {_enemy.Name}!", false);
        }

        public void RunTurn(Action playerAction)
        {
            if (_combatOver) return;

            // 1. Player acts (This calls Defend() or Attack())
            playerAction?.Invoke();

            if (CheckEnd()) return;

            // 2. Enemy acts (This is where the real-time minigame happens)
            EnemyTurn();

            // 3. Post-turn cleanup: Remove the temporary defense boost
            if (_player.IsDefending)
            {
                _player.BaseDefense -= 5;
                _player.IsDefending = false;
            }

            CheckEnd();
        }

        public bool CheckEnd()
        {
            if (_player.HP <= 0)
            {
                _combatOver = true;
                _log("You died.");
                return true;
            }

            if (_enemy.HP <= 0)
            {
                ProcessLoot();
                _combatOver = true;
                _log($"You defeated the {_enemy.Name}!");
                return true;
            }
            return false;
        }

        public void SkipEnemyTurn()
        {
            // Reset the defense boost so it's not permanent
            if (_player.IsDefending)
            {
                _player.BaseDefense -= 5;
                _player.IsDefending = false;
            }
            // We do NOT call EnemyTurn() here, effectively skipping it
        }


        /// <summary>
        /// Timed Press minigame — a bar fills left to right over the time limit.
        /// The fill color shifts green → yellow → red as time runs out.
        /// Player must press the expected key before the bar fills completely.
        /// </summary>
        /// <param name="expectedKey">The key the player must press to succeed.</param>
        /// <param name="timeLimitMs">Time limit in milliseconds before auto-fail.</param>
        /// <returns>True if the player pressed the correct key in time.</returns>
        private bool TimedInput(char expectedKey, int timeLimitMs = 2000)
        {
            while (Console.KeyAvailable) Console.ReadKey(intercept: true);

            var start = DateTime.Now;
            int totalBarLength = 30;

            Console.WriteLine();
            int cursorTop = Console.CursorTop;
            Console.WriteLine();

            while ((DateTime.Now - start).TotalMilliseconds < timeLimitMs)
            {
                double elapsed = (DateTime.Now - start).TotalMilliseconds;
                double percent = elapsed / timeLimitMs;
                int filled = (int)(percent * totalBarLength);

                // Fill color shifts green → yellow → red as urgency increases
                string fillColor = percent < 0.5 ? TextColor.Green
                                 : percent < 0.8 ? TextColor.Yellow
                                 : TextColor.Red;

                Console.SetCursorPosition(0, cursorTop);
                Console.Write($"  {TextColor.Cyan}▐{TextColor.Reset}");

                for (int i = 0; i < totalBarLength; i++)
                    Console.Write(i < filled
                        ? $"{fillColor}█{TextColor.Reset}"
                        : $"{TextColor.Gray}░{TextColor.Reset}");

                Console.Write($"{TextColor.Cyan}▌{TextColor.Reset}  PRESS {TextColor.Yellow}{TextColor.Bold}{char.ToUpper(expectedKey)}{TextColor.Reset}!    ");

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).KeyChar;
                    ClearLine(cursorTop);
                    return char.ToLower(key) == char.ToLower(expectedKey);
                }

                Thread.Sleep(20);
            }

            ClearLine(cursorTop);
            return false;
        }

        /// <summary>
        /// Clears a single console line by overwriting it with spaces,
        /// then repositions the cursor at the start of that line.
        /// </summary>
        /// <param name="row">The console row index to clear.</param>
        private void ClearLine(int row)
        {
            Console.SetCursorPosition(0, row);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, row);
        }

        /// <summary>
        /// Mash minigame — a progress bar fills as the player repeatedly presses the key.
        /// Player must hit the key enough times to reach the goal before time runs out.
        /// </summary>
        /// <param name="keyToMash">The key the player must repeatedly press.</param>
        /// <param name="goalStrikes">Number of successful presses needed to win.</param>
        /// <param name="timeLimitMs">Time limit in milliseconds before auto-fail.</param>
        /// <returns>True if the player reached the goal number of presses in time.</returns>
        private bool MashInput(char keyToMash, int goalStrikes, int timeLimitMs = 3000)
        {
            while (Console.KeyAvailable) Console.ReadKey(true);

            var start = DateTime.Now;
            int currentStrikes = 0;
            int totalBarLength = 30;

            Console.WriteLine();
            int cursorTop = Console.CursorTop;
            Console.WriteLine();

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
                    return true;
                }

                int filled = (int)((double)currentStrikes / goalStrikes * totalBarLength);

                Console.SetCursorPosition(0, cursorTop);
                Console.Write($"  {TextColor.Cyan}▐{TextColor.Reset}");

                for (int i = 0; i < totalBarLength; i++)
                    Console.Write(i < filled
                        ? $"{TextColor.Green}█{TextColor.Reset}"
                        : $"{TextColor.Gray}░{TextColor.Reset}");

                Console.Write(
                    $"{TextColor.Cyan}▌{TextColor.Reset}  MASH " +
                    $"{TextColor.Yellow}{TextColor.Bold}{char.ToUpper(keyToMash)}{TextColor.Reset}!" +
                    $"  {TextColor.Gray}({currentStrikes}/{goalStrikes}){TextColor.Reset}   "
                );

                Thread.Sleep(20);
            }

            ClearLine(cursorTop);
            return false;
        }

        /// <summary>
        /// Sweet Spot minigame — a marker bounces back and forth across the bar.
        /// A green safe zone and white target marker are displayed.
        /// Player must press the key when the moving marker lands inside the green zone.
        /// </summary>
        /// <param name="stopKey">The key the player must press to attempt a hit.</param>
        /// <param name="targetPercent">Center of the safe zone as a 0.0-1.0 position along the bar.</param>
        /// <param name="threshold">Half-width of the safe zone as a 0.0-1.0 fraction of the bar.</param>
        /// <param name="speedMs">Time in milliseconds for one full bounce cycle.</param>
        /// <returns>True if the player pressed the key while the marker was inside the safe zone.</returns>
        private bool SweetSpotInput(char stopKey, double targetPercent = 0.5, double threshold = 0.1, int speedMs = 1500)
        {
            while (Console.KeyAvailable) Console.ReadKey(true);

            var start = DateTime.Now;
            int totalBarLength = 30;

            int targetIndex = (int)(targetPercent * totalBarLength);
            int halfZoneWidth = Math.Max(1, (int)(threshold * totalBarLength));
            int zoneStart = Math.Max(0, targetIndex - halfZoneWidth);
            int zoneEnd = Math.Min(totalBarLength - 1, targetIndex + halfZoneWidth);

            Console.WriteLine();
            int cursorTop = Console.CursorTop;
            Console.WriteLine();

            while (true)
            {
                double elapsed = (DateTime.Now - start).TotalMilliseconds;
                double progress = (Math.Sin(elapsed / speedMs * Math.PI * 2) + 1) / 2;
                int markerIndex = (int)(progress * totalBarLength);

                Console.SetCursorPosition(0, cursorTop);
                Console.Write($"  {TextColor.Cyan}▐{TextColor.Reset}");

                for (int i = 0; i < totalBarLength; i++)
                {
                    if (i == markerIndex)
                        Console.Write($"{TextColor.Cyan}{TextColor.Bold}▌{TextColor.Reset}");   // Thin cyan marker
                    else if (i == targetIndex)
                        Console.Write($"{TextColor.White}{TextColor.Bold}▓{TextColor.Yellow}");  // Exact target
                    else if (i >= zoneStart && i <= zoneEnd)
                        Console.Write($"{TextColor.Green}▓{TextColor.Reset}");                  // Safe zone — medium block
                    else
                        Console.Write($"{TextColor.Gray}▒{TextColor.Reset}");                   // Dead zone — light shade
                }

                Console.Write($"{TextColor.Cyan}▌{TextColor.Reset}  PRESS {TextColor.Yellow}{TextColor.Bold}{char.ToUpper(stopKey)}{TextColor.Reset}!   ");

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).KeyChar;
                    if (char.ToLower(key) == char.ToLower(stopKey))
                    {
                        bool hit = markerIndex >= zoneStart && markerIndex <= zoneEnd;
                        ClearLine(cursorTop);
                        return hit;
                    }
                }

                Thread.Sleep(15);
            }
        }



        /// <summary>
        /// Sequence minigame — the player must perform a timed press multiple times in a row.
        /// Each press must succeed within its individual time limit.
        /// Failing any single press in the sequence counts as a full failure.
        /// </summary>
        /// <param name="key">The key the player must press for each step.</param>
        /// <param name="count">Number of successful presses required to complete the sequence.</param>
        /// <param name="timeLimitPerPress">Time limit in milliseconds for each individual press.</param>
        /// <returns>True if all presses in the sequence were successful.</returns>
        private bool SequenceInput(char key, int count, int timeLimitPerPress = 1000)
        {
            for (int i = 0; i < count; i++)
            {
                _log($"{TextColor.Gray}({i + 1}/{count}){TextColor.Reset} QUICK! PRESS {TextColor.Yellow}{TextColor.Bold}{char.ToUpper(key)}{TextColor.Reset}!", false);

                if (!TimedInput(key, timeLimitPerPress))
                    return false;

                Thread.Sleep(100);
            }
            return true;
        }

        private bool ChainedHitBarInput(string data, int totalTime, int barWidth = 40)
        {
            // Parse target positions from XML Data attribute
            var targets = data.Split(',')
                .Select(s => new SweetspotTarget
                {
                    Center = float.Parse(s.Trim()),
                    Width = 0.08f,
                    IsHit = false
                }).ToList();

            DateTime startTime = DateTime.Now;
            float cursor = 0f;

            // Flush the buffer to prevent instant fails from menu selection
            while (Console.KeyAvailable) Console.ReadKey(true);

            while (cursor < 1.0f)
            {
                float elapsed = (float)(DateTime.Now - startTime).TotalMilliseconds;
                cursor = elapsed / totalTime;

                RenderChainedBar(cursor, targets, barWidth);

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Spacebar)
                    {
                        // Check if the cursor is inside ANY target zone that hasn't been hit
                        for (int i = 0; i < targets.Count; i++)
                        {
                            var t = targets[i];
                            if (!t.IsHit && Math.Abs(cursor - t.Center) < t.Width / 2)
                            {
                                t.IsHit = true;
                                targets[i] = t;
                                break;
                            }
                        }
                    }
                }
                Thread.Sleep(15);
            }

            // Success if all zones were tapped
            return targets.All(t => t.IsHit);
        }

        private void RenderChainedBar(float cursor, List<SweetspotTarget> targets, int width)
        {
            Console.SetCursorPosition(0, Console.CursorTop);

            // Instruction Line
            Console.Write($"{TextColor.Cyan}  [ SPACE ]{TextColor.Reset} to HIT!  ");

            Console.Write("[");
            for (int i = 0; i < width; i++)
            {
                float pos = (float)i / width;
                bool isCursor = Math.Abs(pos - cursor) < (1.5f / width);
                var target = targets.FirstOrDefault(t => Math.Abs(pos - t.Center) < t.Width / 2);

                if (isCursor)
                    Console.Write($"{TextColor.White}|{TextColor.Reset}");
                else if (target.Center != 0)
                    Console.Write(target.IsHit ? $"{TextColor.Green}█{TextColor.Reset}" : $"{TextColor.Red}▒{TextColor.Reset}");
                else
                    Console.Write($"{TextColor.Gray}-{TextColor.Reset}");
            }

            int hits = targets.Count(t => t.IsHit);
            Console.Write($"]  {TextColor.Yellow}{hits}/{targets.Count}{TextColor.Reset}  ");
        }

        public void EnemyTurn()
        {
            var move = _enemy.Moves[new Random().Next(_enemy.Moves.Count)];

            _log($"{TextColor.Red}{_enemy.Name}{TextColor.Reset} uses {TextColor.Yellow}{move.Name}{TextColor.Reset}!", false);

            System.Threading.Thread.Sleep(3000);

            bool success = false;

            switch (move.Type)
            {
                case "Timed": success = TimedInput(move.Key, move.TimeLimit); break;
                case "Mash": success = MashInput(move.Key, move.Goal, move.TimeLimit); break;
                case "SweetSpot": success = SweetSpotInput(move.Key, move.Target, move.Threshold); break;
                case "Sequence": success = SequenceInput(move.Key, move.Count); break;
                case "ChaineHitBar": success = ChainedHitBarInput(move.ChainedHitBarPositions, move.TimeLimit); break;
            }

            if (success)
            {
                _log($"{TextColor.Green}Successfully countered {move.Name}!{TextColor.Reset}");

                #region TUTORIAL CONTENT
                // Only trigger this once the counter is confirmed successful
                if (_tutorial.IsActive && _tutorial.CurrentState == TutorialState.TutForceDefend)
                {
                    _log("You blocked! Now try to attack.", true);
                    _tutorial.SetState(TutorialState.TutSuccessfullyDefended);
                }
                #endregion
            }

            else
            {
                _player.TakeDamage(_enemy.Attack);
                _log($"{TextColor.Red}Failed! You took {_enemy.Attack} damage!{TextColor.Reset}");
                if(_tutorial.IsActive && _tutorial.CurrentState == TutorialState.TutForceDefend)
                    _tutorial.SetState(TutorialState.TutFailedDefend);

                #region TUTORIAL CONTENT
                if (_tutorial.IsActive && _tutorial.CurrentState == TutorialState.TutFailedDefend)
                {
                    _log("You missed the timing! Don't worry, try to block the next one.", true);
                    _tutorial.SetState(TutorialState.TutForceDefend);
                }
                #endregion
            }
        }

        private void ProcessLoot()
        {
            Random rng = new Random();
            var def = EnemyDatabase.Enemies.Values.First(e => e.Name == _enemy.Name);
            int goldDropped = rng.Next(def.MinGold, def.MaxGold + 1);

            _player.Gold += goldDropped;
            _log($"{TextColor.Yellow}The {_enemy.Name} dropped {goldDropped} gold!{TextColor.Reset}", true);

            #region TUTORIAL CONTENT
            if (_tutorial.IsActive && _enemy.Name == "Green Slime")
            {
                // Force the Torch drop instead of rolling the table
                _player.Inventory.Add(ItemId.Torch, 1);
                _log($"{TextColor.Cyan}Slime Dropped A Torch!{TextColor.Reset}", true);
                return;
            }
            #endregion

            foreach (var loot in def.LootTable)
            {
                if (rng.NextDouble() <= loot.Chance)
                {
                    _player.Inventory.Add(loot.Id, 1);
                    var itemDef = ItemDatabase.Items[loot.Id];
                    _log($"{TextColor.Cyan}LOOTED: {itemDef.Name}!{TextColor.Reset}", true);
                }
            }
        }

        private void TakeDamage()
        {
            _player.HP -= _enemy.Attack;
            _log($"{TextColor.Red}You took damage!{TextColor.Reset}");
        }

        public void Attack()
        {
            _enemy.HP -= _player.Attack;
            _log($"You attacked {TextColor.Red}{_enemy.Name}{TextColor.Reset} for {TextColor.Yellow}{_player.Attack}{TextColor.Reset} damage!");

            #region TUTORIAL CONTENT
            if (_tutorial.IsActive && _tutorial.CurrentState == TutorialState.TutForceAttack)
            {
                _log("Great hit! Now watch out, the Slime is counter-attacking!", true);
                _tutorial.SetState(TutorialState.TutSuccessfullyAttacked);
            }
            #endregion
        }

        public void Defend()
        {
            _player.IsDefending = true;
            _player.BaseDefense += 5;

            //#region TUTORIAL CONTENT
            //if (_tutorial.IsActive && _tutorial.CurrentState == TutorialState.TutForceDefend)
            //{
            //    _tutorial.SetState(TutorialState.TutSuccessfullyDefended);
            //}
            //#endregion
        }

        public void UseSkill(SkillId id)
        {
            var skill = SkillDatabase.Skills[id];

            // 1. Validation
            foreach (var cost in skill.ResourceCosts)
            {
                bool hasEnough = cost.ResourceType switch
                {
                    "Mana" => _player.BaseMana >= cost.Amount,
                    "Stamina" => _player.Stamina >= cost.Amount,
                    "HP" => _player.HP > cost.Amount,
                    _ => true
                };

                if (!hasEnough)
                {
                    _log($"{TextColor.Red}Not enough {cost.ResourceType}!{TextColor.Reset}");
                    return;
                }
            }

            // --- DEDUCTION POINT ---
            foreach (var cost in skill.ResourceCosts)
            {
                if (cost.ResourceType == "Mana") _player.BaseMana -= cost.Amount;
                else if (cost.ResourceType == "Stamina") _player.Stamina -= cost.Amount;
                else if (cost.ResourceType == "HP") _player.HP -= cost.Amount;
            }

            bool totalSuccess = true;

            // 2. Run Mini-games
            foreach (var step in skill.Steps)
            {
                bool stepSuccess = step.Type switch
                {
                    "Mash" => MashInput(step.Key, step.Goal, step.Time),
                    "Timed" => TimedInput(step.Key, step.Time),
                    "Sequence" => SequenceInput(step.Key, step.Goal, step.Time),
                    "ChainedHitBar" => ChainedHitBarInput(step.ChainedHitBarPositions, step.Time),
                    _ => true
                };

                if (!stepSuccess)
                {
                    _log($"{TextColor.Red}{step.FailMsg}{TextColor.Reset}");
                    totalSuccess = false;
                    break;
                }
            }

            // 3. Execution & Tutorial Interception
            if (totalSuccess)
            {
                ApplySkillEffects(skill);
                _log($"{TextColor.Green}Success!{TextColor.Reset} Executed {skill.Name}.");

                #region TUTORIAL CONTENT
                if (_tutorial.IsActive && (_tutorial.CurrentState == TutorialState.TutForceSkills || _tutorial.CurrentState == TutorialState.TutSelectedSkills))
                {
                    _log("Devastating! That's how you use skills to end a fight.", true);
                    _tutorial.SetState(TutorialState.TutSuccessfullyUsedSkill);
                    _enemy.HP = 0; // Forced win
                }
                #endregion
            }
            else
            {
                _log($"{TextColor.Red}Skill Failed! Resources wasted.{TextColor.Reset}");

                #region TUTORIAL CONTENT
                if (_tutorial.IsActive)
                {
                    // 1. REFUND RESOURCES: Prevent soft-lock if they fail the minigame
                    foreach (var cost in skill.ResourceCosts)
                    {
                        if (cost.ResourceType == "Mana") _player.BaseMana += cost.Amount;
                        else if (cost.ResourceType == "Stamina") _player.Stamina += cost.Amount;
                    }

                    // 2. RESET STATE: Force them back to the specific skill gating
                    _tutorial.SetState(TutorialState.TutForceSkills);
                    _log("Momentum lost! Focus and try again.", true);

                    // Note: The GameEngine loop should call SetCombatMenu() next 
                    // to fix the [1-5] menu back to the [1-1] Skill menu.
                }
                #endregion
            }
        }

        private void ApplySkillEffects(SkillDef skill)
        {
            switch (skill.SkillType)
            {
                case "Damage":
                    int damage = _player.Attack + skill.Value;
                    _enemy.HP -= damage;
                    _log($"Dealt {TextColor.Yellow}{damage}{TextColor.Reset} damage to {TextColor.Red}{_enemy.Name}{TextColor.Reset}!");
                    break;

                case "Healing":
                    _player.HP = Math.Min(_player.MaxHP, _player.HP + skill.Value);
                    _log($"Restored {TextColor.Green}{skill.Value}{TextColor.Reset} HP!");
                    break;

                case "Mana":
                    _player.BaseMana += skill.Value;
                    _log($"Restored {TextColor.Cyan}{skill.Value}{TextColor.Reset} Mana!");
                    break;
            }

            //// Apply Stun logic if defined in XML
            //if (skill.StunDuration > 0)
            //{
            //    // Assuming you add these properties to your Enemy model
            //    _enemy.IsStunned = true;
            //    _enemy.StunTurns = skill.StunDuration;
            //    _log($"{TextColor.Magenta}{_enemy.Name} is stunned for {skill.StunDuration} turns!{TextColor.Reset}");
            //}
        }

        public bool TryFlee()
        {
            return new Random().Next(100) < 50;
        }
    }
}