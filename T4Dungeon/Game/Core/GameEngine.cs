using T4Dungeon.Game.Models;
using T4Dungeon.Game.Systems;
using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Core
{
    public class GameEngine
    {
        private GameState _state;

        private Player _player;
        private MapManager _mapManager;
        private UIContext _ui;
        private CombatSystem _combat;

        private readonly List<string> _messages = new(); 
        private bool _showInventory = false;

        public void Run()
        {
            _state = GameState.NewGame;

            while (_state != GameState.Exit)
            {
                switch (_state)
                {
                    case GameState.NewGame:
                        InitGame();
                        SetMainMenu();
                        break;

                    case GameState.Running:
                        ConsoleRenderer.Render(_mapManager, _ui, _messages, _player, _showInventory, false);
                        _messages.Clear();
                        HandleInput();
                        break;

                    case GameState.Combat:
                        RunCombatLoop();
                        break;
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            _state = GameState.Exit;
        }


        private void InitGame()
        {
            //Init systems
            _player = new Player();
            _mapManager = new MapManager(10, 10);

            //Change state
            _state = GameState.Running;

            //Test items
            _player.Inventory.Add(ItemId.IronSword, 1);
        }

        private void SetMainMenu()
        {
            _ui = new UIContext();

            _ui.Options = new List<MenuOption>
            {
                new MenuOption { Text = "Move", Action = SetMoveMenu },
                new MenuOption { Text = "Equipment", Action = SetEquiptMenu },
                new MenuOption { Text = "Open Inventory", Action = SetInventoryMenu },
                new MenuOption { Text = "Interact", Action = null, IsImplemented = false },
                new MenuOption { Text = "Exit Game", Action = () => _state = GameState.Exit }
            };
        }

        private void SetMoveMenu() 
        {
            _ui.Options = new List<MenuOption>
            {
                new MenuOption { Text = "Up", Action = () => MovePlayer(0, -1) },
                new MenuOption { Text = "Down", Action = () => MovePlayer(0, 1) },
                new MenuOption { Text = "Left", Action = () => MovePlayer(-1, 0) },
                new MenuOption { Text = "Right", Action = () => MovePlayer(1, 0) },
                new MenuOption { Text = "Back", Action = SetMainMenu }
            };

        }

        private void SetInventoryMenu()
        {

            _showInventory = true;
            _ui.Options = new List<MenuOption>();

            int index = 1;

            foreach (var item in _player.Inventory.Items)
            {
                var def = ItemDatabase.Items[item.ItemId];

                _ui.Options.Add(new MenuOption
                {
                    Text = $" {def.Name} \tx{item.Amount} \t- {def.Description}",
                    Action = () => Log("Use not implemented"),
                    IsImplemented = false
                });

                index++;
            }

            _ui.Options.Add(new MenuOption
            {
                Text = "Back",
                Action = () =>
                {
                    _showInventory = false;
                    SetMainMenu();
                }
            });
        }

        private void SetCombatMenu()
        {
            _ui.Options = new List<MenuOption>
            {
                new MenuOption { Text = "Attack", Action = () => _combat.RunTurn(_combat.Attack) },

                new MenuOption { Text = "Defend", Action = () => _combat.RunTurn(_combat.Defend) },

                new MenuOption
                {
                    Text = "Attempt Flee",
                    Action = () =>
                    {
                        bool escaped = _combat.TryFlee();
                        Console.WriteLine(escaped ? "You escaped!" : "Failed to escape!");
                        Log(escaped ? "You escaped!" : "Failed to escape!");
                        if (escaped)
                            _state = GameState.Running;
                    }
                },

                new MenuOption { Text = "Open Inventory", Action = SetInventoryMenu },
            };
        }

        private void SetEquiptMenu()
        {
            _ui.Options = new List<MenuOption>();

            var slotsToShow = new[] { EquiptSlot.Weapon, EquiptSlot.Armor, EquiptSlot.Accessory };

            foreach (var slot in slotsToShow)
            {
                _player.Equipment.TryGetValue(slot, out var currentId);
                string currentItemName = currentId.HasValue
                    ? ItemDatabase.Items[currentId.Value].Name
                    : "Empty";

                _ui.Options.Add(new MenuOption
                {
                    Text = $"{slot}: {currentItemName}",
                    Action = () => SetItemSelectMenu(slot)
                });
            }

            _ui.Options.Add(new MenuOption { Text = "Back", Action = SetMainMenu });
        }

        private void SetItemSelectMenu(EquiptSlot slot)
        {
            _ui.Options = new List<MenuOption>();

            // Filter inventory for items matching the Slot tag
            var validItems = _player.Inventory.Items
                .Select(i => ItemDatabase.Items[i.ItemId])
                .Where(def => def.Slot == slot)
                .Take(9) // Limit to 9 items 
                .ToList();

            if (validItems.Count == 0)
            {
                Log($"No equippable {slot} items found!");
                SetEquiptMenu(); // Go back immediately
                return;
            }

            foreach (var item in validItems)
            {
                _ui.Options.Add(new MenuOption
                {
                    Text = item.Name,
                    Action = () => {
                        EquipItem(slot, item.Id);
                        SetEquiptMenu();
                    }
                });
            }

            _ui.Options.Add(new MenuOption { Text = "Back", Action = SetEquiptMenu });
        }

        private void EquipItem(EquiptSlot slot, ItemId newItemId)
        {
            _player.Equipment[slot] = newItemId;

            var newDef = ItemDatabase.Items[newItemId];
            Log($"Equipped {newDef.Name}!");
        }


        private void SetSkillMenu()
        {
            _ui.Options = new List<MenuOption>
            {
                new MenuOption { Text = "Back", Action = SetCombatMenu }
            };
        }

        private void CheckCombatState()
        {
            if (_combat.IsOver)
            {
                Log(_combat.Message);
                _state = GameState.Running;
                SetMainMenu();
            }
        }

        private void MovePlayer(int dx, int dy)
        {
            if (!ValidateMove(dx, dy)) return;

            
            var currentPos = _mapManager.PlayerPosition;
            _mapManager.PlayerPosition = new Vector2Int(currentPos.X + dx, currentPos.Y + dy);
            var cell = _mapManager.Grid[_mapManager.PlayerPosition.X, _mapManager.PlayerPosition.Y];
            cell.Explored = true;

            
            InteractWithCell(cell);
        }
        private void RunCombatLoop()
        {
            ConsoleRenderer.Render(_mapManager, _ui, _messages, _player, false, true, _combat.Enemy);

            HandleInput();

            if (_combat.IsOver)
            {
                _state = GameState.Running;
                SetMainMenu();
            }
        }

        private void InteractWithCell(Cell cell)
        {
            
            string eventMsg = cell.Event.Execute(_player, cell);

            
            switch (cell.Type)
            {
                case CellType.Exit:
                    Log("You found the exit! The light of the outside world blinds you...", true);
                    Log("YOU WIN!", true);
                    _state = GameState.Exit; // This will break the while loop in Run()
                    break;

                case CellType.Combat:
                   
                    Log(eventMsg, true);
                    StartCombatTransition();
                    break;

                case CellType.Treasure:
                    Log(eventMsg, true);
                    ClearCell(cell); 
                    break;

                default:
                    if (!string.IsNullOrEmpty(eventMsg)) Log(eventMsg, false);
                    break;
            }
        }

        private void StartCombatTransition()
        {
            var randomId = (EnemyId)new Random().Next(2001, 2004);
            _combat = new CombatSystem(_player, new Enemy(randomId), Log);
            _state = GameState.Combat;
            SetCombatMenu();
        }

        private void ClearCell(Cell cell)
        {
            cell.Type = CellType.Empty;
            cell.Event = CellEventFactory.Create(CellType.Empty);
        }

        private bool ValidateMove(int dx, int dy)
        {
            var pos = _mapManager.PlayerPosition;
            var newPos = new Vector2Int(pos.X + dx, pos.Y + dy);
            if (newPos.X < 0 || newPos.X >= _mapManager.Grid.GetLength(0) ||
                newPos.Y < 0 || newPos.Y >= _mapManager.Grid.GetLength(1))
            {
                Log("You can't move outside the map.");
                return false;
            }
            return true;    
        }

        private void HandleInput()
        {
            int blinkStage = 0;
            // We'll keep track of where we are so we can overwrite the same line
            int promptLine = Console.CursorTop;

            while (!Console.KeyAvailable)
            {
                Console.SetCursorPosition(0, promptLine);

                // Slightly abrasive color: DarkCyan or Magenta
                Console.ForegroundColor = (blinkStage % 2 == 0) ? ConsoleColor.DarkCyan : ConsoleColor.Cyan;

                Console.Write(" >> CHOOSE AN OPTION [1-" + _ui.Options.Count + "] <<   ");
                Console.ResetColor();

                Thread.Sleep(400); // Pulse speed
                blinkStage++;
            }

            // Once a key is pressed, handle it as before
            var key = Console.ReadKey(true);
            ClearLine(promptLine); // Clean up the prompt before moving on

            int index = key.KeyChar - '1';

            if (index < 0 || index >= _ui.Options.Count)
            {
                Log("Invalid Option", true);
                return;
            }

            var option = _ui.Options[index];

            if (!option.IsImplemented)
            {
                Log("Option not implemented.");
                return;
            }

            option.Action?.Invoke();
        }

        // Simple helper to keep the console clean
        private void ClearLine(int row)
        {
            Console.SetCursorPosition(0, row);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, row);
        }

        private void Log(string msg, bool waitForKey = true)
        {
            _messages.Add(msg);
            if (_messages.Count > 10) _messages.RemoveAt(0);

            bool isCombat = (_state == GameState.Combat);
            Enemy? currentEnemy = isCombat ? _combat?.Enemy : null;

            ConsoleRenderer.Render(_mapManager, _ui, _messages, _player, _showInventory, isCombat, currentEnemy);

            if (waitForKey)
            {
                Console.WriteLine("\n -- Press any key to continue --");
                Console.ReadKey(true);
                while (Console.KeyAvailable) Console.ReadKey(true);
            }
        }

        private void Log(string msg)
        {
            Log(msg, false); 
        }
    }
}
