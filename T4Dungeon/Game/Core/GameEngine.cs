using T4Dungeon.Game.Models;
using T4Dungeon.Game.Systems;
using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;
using static T4Dungeon.Game.Models.ShopSlot;

namespace T4Dungeon.Game.Core
{
    public class GameEngine
    {
        #region Fields & Properties
        private GameState _state;

        private Player _player;
        private MapManager _mapManager;
        private UIContext _ui;
        private CombatSystem _combat;

        private readonly List<string> _messages = new();
        private bool _showInventory = false;
        private ShopInstance _currentShop;
        #endregion

        #region Core Loop
        /// <summary>
        /// Starts the game loop and handles top-level state transitions.
        /// </summary>
        public void Run()
        {
            _state = GameState.StartScreen;
            SetStartScreen();

            while (_state != GameState.Exit)
            {
                switch (_state)
                {
                    case GameState.StartScreen:
                        // we pass null for map and player as they aren't initialized yet
                        ConsoleRenderer.Render(null, _ui, _messages, null, false, false);
                        HandleInput();
                        break;

                    case GameState.Tutorial:
                        RunTutorialLoop();
                        break;

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

                    case GameState.Shop:
                        ConsoleRenderer.Render(null, _ui, _messages, _player, false, false, null);
                        HandleInput();
                        break;
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            _state = GameState.Exit;
        }

        /// <summary>
        /// Initializes the player, map, and starting items.
        /// </summary>
        private void InitGame()
        {
            //Init systems
            _player = new Player();
            _mapManager = new MapManager(10, 10);

            //Change state
            _state = GameState.Running;

            //Test items
            _player.Inventory.Add(ItemId.IronSword, 1);
            _player.Inventory.Add(ItemId.HealthPotion, 1);
        }
        #endregion


        #region Start Screen Logic

        /// <summary>
        /// Configures the initial entry menu for the game.
        /// </summary>
        private void SetStartScreen()
        {
            _ui = new UIContext
            {
                // You could also add this to MenuFactory if you prefer
                Options = new List<MenuOption>
        {
            new MenuOption
            {
                Text = "Start Game",
                Action = () => { _state = GameState.NewGame; }
            },
            new MenuOption
            {
                Text = "Tutorial",
                Action = () => { _state = GameState.Tutorial; }
            },
            new MenuOption
            {
                Text = "Dev Options",
                IsImplemented = false
            },
            new MenuOption
            {
                Text = "Exit",
                Action = () => _state = GameState.Exit
            }
        }
            };
        }

        /// <summary>
        /// Placeholder for the tutorial logic loop.
        /// </summary>
        private void RunTutorialLoop()
        {
            Log("Tutorial coming soon...", true);
            _state = GameState.StartScreen;
            SetStartScreen();
        }

        #endregion

        #region Menu Construction
        /// <summary>
        /// Sets up the main exploration menu.
        /// </summary>
        private void SetMainMenu()
        {
            _ui = MenuFactory.CreateMainMenu(
                onMove: SetMoveMenu,
                onEquip: SetEquiptMenu,
                onInv: SetInventoryMenu,
                onExit: () => _state = GameState.Exit
            );
        }

        /// <summary>
        /// Sets up the movement direction menu.
        /// </summary>
        private void SetMoveMenu()
        {
            _ui = MenuFactory.CreateMoveMenu(
                up: () => MovePlayer(0, -1),
                down: () => MovePlayer(0, 1),
                left: () => MovePlayer(-1, 0),
                right: () => MovePlayer(1, 0),
                back: SetMainMenu
            );
        }

        /// <summary>
        /// Sets up the inventory display and usage menu.
        /// </summary>
        private void SetInventoryMenu()
        {
            _showInventory = true;
            _ui = MenuFactory.CreateInventoryMenu(
                _player,
                onUse: UseItem,
                onBack: () => {
                    _showInventory = false;
                    if (_state == GameState.Combat) SetCombatMenu();
                    else SetMainMenu();
                }
            );
        }

        /// <summary>
        /// Sets up the combat action menu.
        /// </summary>
        private void SetCombatMenu()
        {
            _ui = MenuFactory.CreateCombatMenu(
                onAttack: () => _combat.RunTurn(_combat.Attack),
                onDefend: () => _combat.RunTurn(_combat.Defend),
                onFlee: AttemptFlee, // Refactored flee logic into a method
                onInv: SetInventoryMenu
            );
        }

        /// <summary>
        /// Sets up the equipment slot selection menu.
        /// </summary>
        private void SetEquiptMenu()
        {
            _ui = MenuFactory.CreateEquipmentMenu(
                _player,
                onSelectSlot: SetItemSelectMenu,
                onBack: SetMainMenu
            );
        }

        /// <summary>
        /// Sets up the item selection menu for a specific equipment slot.
        /// </summary>
        private void SetItemSelectMenu(EquiptSlot slot)
        {
            _ui = MenuFactory.CreateItemSelectMenu(
                _player,
                slot,
                onEquip: (id) => { EquipItem(slot, id); SetEquiptMenu(); },
                onBack: SetEquiptMenu
            );
        }

        /// <summary>
        /// Placeholder for combat skill menu.
        /// </summary>
        private void SetSkillMenu()
        {
            _ui.Options = new List<MenuOption>
            {
                new MenuOption { Text = "Back", Action = SetCombatMenu }
            };
        }
        #endregion

        #region Player Actions & Combat
        /// <summary>
        /// Handles player equipment logic.
        /// </summary>
        private void EquipItem(EquiptSlot slot, ItemId newItemId)
        {
            _player.Equipment[slot] = newItemId;

            var newDef = ItemDatabase.Items[newItemId];
            Log($"Equipped {newDef.Name}!");
        }

        /// <summary>
        /// Checks if combat has concluded.
        /// </summary>
        private void CheckCombatState()
        {
            if (_combat.IsOver)
            {
                Log(_combat.Message);
                _state = GameState.Running;
                SetMainMenu();
            }
        }

        /// <summary>
        /// Main combat logic loop.
        /// </summary>
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

                private static readonly List<(EnemyId id, int weight)> _spawnTable = new()
        {
            (EnemyId.Slime,  0),
            (EnemyId.Goblin, 100),
            (EnemyId.Orc,    0),
        };

        private EnemyId GetRandomEnemy()
        {
            int total = _spawnTable.Sum(e => e.weight);
            int roll = new Random().Next(total);
            int cumulative = 0;

            foreach (var (id, weight) in _spawnTable)
            {
                cumulative += weight;
                if (roll < cumulative) return id;
            }

            return _spawnTable[0].id;
        }

        /// <summary>
        /// Transitions the game state into a combat encounter.
        /// </summary>
        private void StartCombatTransition()
        {
            var randomId = GetRandomEnemy();
            _combat = new CombatSystem(_player, new Enemy(randomId), Log);
            _state = GameState.Combat;
            SetCombatMenu();
        }

        /// <summary>
        /// Handles the logic for attempting to flee from battle.
        /// </summary>
        private void AttemptFlee()
        {
            if (_combat.TryFlee())
            {
                _showInventory = false;
                _state = GameState.Running;
                SetMainMenu();
                Log("You escaped!");
            }
            else
            {
                Log("Failed to escape!");
                _combat.EnemyTurn();
            }
        }
        #endregion

        #region Movement & Exploration
        /// <summary>
        /// Handles player movement and triggers map interactions.
        /// </summary>
        private void MovePlayer(int dx, int dy)
        {
            if (!ValidateMove(dx, dy)) return;

            var currentPos = _mapManager.PlayerPosition;
            _mapManager.PlayerPosition = new Vector2Int(currentPos.X + dx, currentPos.Y + dy);
            var cell = _mapManager.Grid[_mapManager.PlayerPosition.X, _mapManager.PlayerPosition.Y];
            cell.Explored = true;

            InteractWithCell(cell);
        }

        /// <summary>
        /// Evaluates the cell type and executes the corresponding event.
        /// </summary>
        private void InteractWithCell(Cell cell)
        {
            string eventMsg = cell.Event.Execute(_player, cell);

            switch (cell.Type)
            {
                case CellType.Exit:
                    Log("You found the exit! The light of the outside world blinds you...", true);
                    Log("YOU WIN!", true);
                    _state = GameState.Exit;
                    break;

                case CellType.Combat:
                    Log(eventMsg, true);
                    StartCombatTransition();
                    break;

                case CellType.Treasure:
                    Log(eventMsg, true);
                    ClearCell(cell);
                    break;

                case CellType.Shop:
                    Log(eventMsg, true);
                    GenerateShop();
                    _state = GameState.Shop;
                    break;

                default:
                    if (!string.IsNullOrEmpty(eventMsg)) Log(eventMsg, false);
                    break;
            }
        }

        /// <summary>
        /// Clears an event from a map cell once completed.
        /// </summary>
        private void ClearCell(Cell cell)
        {
            cell.Type = CellType.Empty;
            cell.Event = CellEventFactory.Create(CellType.Empty);
        }

        /// <summary>
        /// Ensures movement stays within map boundaries.
        /// </summary>
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
        #endregion

        #region Item Usage
        private void UseItem(ItemId id)
        {
            string logResult = InventorySystem.UseItem(_player, id);

            if (!string.IsNullOrEmpty(logResult))
            {
                Log(logResult);
            }

            if (_state == GameState.Combat)
            {
                _combat.EnemyTurn();
                SetCombatMenu();
            }
            else
            {
                SetInventoryMenu();
            }
        }
        #endregion

        #region Shop Logic
        private void GenerateShop()
        {
            _currentShop = new ShopInstance();
            _currentShop.GenerateInventory();
            SetShopWelcomeMenu();
        }

        private void BuyItem(ShopSlot slot)
        {
            if (_currentShop.PurchaseItem(slot, _player))
            {
                Log($"Purchased {ItemDatabase.Items[slot.ItemId].Name}!", true);
                SetShopBuyMenu();
            }
            else
            {
                Log("Not enough gold!", true);
            }
        }

        private void SetShopWelcomeMenu()
        {
            _ui = MenuFactory.CreateShopWelcomeMenu(onBrowse: SetShopBuyMenu, onLeave: ExitShop);
        }

        private void SetShopBuyMenu()
        {
            _ui = MenuFactory.CreateShopBuyMenu(_currentShop, onBuy: BuyItem, onBack: SetShopWelcomeMenu);
        }

        private void ExitShop()
        {
            _state = GameState.Running;
            ClearCell(_mapManager.Grid[_mapManager.PlayerPosition.X, _mapManager.PlayerPosition.Y]);
            SetMainMenu();
        }
        #endregion

        #region Input & Messaging
        /// <summary>
        /// Handles console input and pulses the prompt.
        /// </summary>
        private void HandleInput()
        {
            int blinkStage = 0;
            int promptLine = Console.CursorTop;

            while (!Console.KeyAvailable)
            {
                Console.SetCursorPosition(0, promptLine);
                Console.ForegroundColor = (blinkStage % 2 == 0) ? ConsoleColor.DarkCyan : ConsoleColor.Cyan;
                Console.Write(" >> CHOOSE AN OPTION [1-" + _ui.Options.Count + "] <<   ");
                Console.ResetColor();
                Thread.Sleep(400);
                blinkStage++;
            }

            var key = Console.ReadKey(true);
            ClearLine(promptLine);

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

        /// <summary>
        /// Clears a line in the console for clean output.
        /// </summary>
        private void ClearLine(int row)
        {
            Console.SetCursorPosition(0, row);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, row);
        }

        /// <summary>
        /// Logs a message and refreshes the console render.
        /// </summary>
        private void Log(string msg, bool waitForKey = true)
        {
            _messages.Add(msg);
            if (_messages.Count > 10) _messages.RemoveAt(0);

            bool isCombat = (_state == GameState.Combat);
            bool isShop = (_state == GameState.Shop);
            Enemy? currentEnemy = isCombat ? _combat?.Enemy : null;
            var mapToRender = isShop ? null : _mapManager;

            ConsoleRenderer.Render(mapToRender, _ui, _messages, _player, _showInventory, isCombat, currentEnemy);
            if (waitForKey)
            {
                Console.WriteLine("\n -- Press any key to continue --");
                Console.ReadKey(true);
                while (Console.KeyAvailable) Console.ReadKey(true);
            }
        }

        /// <summary>
        /// Log overload for messages that don't require key presses.
        /// </summary>
        private void Log(string msg)
        {
            Log(msg, false);
        }
        #endregion
    }
}