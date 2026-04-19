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
                        ConsoleRenderer.Render(_mapManager, _ui, _messages, _player, _showInventory);
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
            var pos = _mapManager.PlayerPosition;


            var newPos = new Vector2Int(pos.X + dx, pos.Y + dy);
            if (!ValidateMove(dx, dy))
                return;

            _mapManager.PlayerPosition = newPos;
            _mapManager.Grid[newPos.X, newPos.Y].Explored = true;

            var cell = _mapManager.Grid[newPos.X, newPos.Y];
            Console.WriteLine(cell.Event.Execute(_player, cell));
            Log(cell.Event.Execute(_player, cell));

            if (cell.Type == CellType.Combat)
            { 
                var randomId = (EnemyId)new Random().Next(2001, 2004);
                _combat = new CombatSystem(_player, new Enemy(randomId), Log);
                _state = GameState.Combat;
                SetCombatMenu();
            }
        }

        private void RunCombatLoop()
        {
            ConsoleRenderer.Render(_mapManager, _ui, _messages, _player, false);

            HandleInput();

            Console.WriteLine(_combat.Message);
            Log(_combat.Message);

            if (_combat.IsOver)
            {
                _state = GameState.Running;
                SetMainMenu();
            }
        }

        private bool ValidateMove(int dx, int dy)
        {
            var pos = _mapManager.PlayerPosition;
            var newPos = new Vector2Int(pos.X + dx, pos.Y + dy);
            if (newPos.X < 0 || newPos.X >= _mapManager.Grid.GetLength(0) ||
                newPos.Y < 0 || newPos.Y >= _mapManager.Grid.GetLength(1))
            {
                Console.WriteLine("You can't move outside the map.");
                Log("You can't move outside the map.");
                return false;
            }
            return true;    
        }

        private void HandleInput()
        {
            while (true)
            {
                var key = Console.ReadKey(true);

                int index = key.KeyChar - '1';

                if (index < 0 || index >= _ui.Options.Count)
                {
                    Console.WriteLine("Invalid option.");
                    Log("Invalid Option");
                    break; 
                }

                var option = _ui.Options[index];

                if (!option.IsImplemented)
                {
                    Console.WriteLine("Option not implemented.");
                    Log("Option not implemented.");
                    break; 
                }
                option.Action?.Invoke();
                break;
            }
        }

        private void Log(string msg, bool waitForKey = true)
        {
            _messages.Add(msg);
            if (_messages.Count > 10) _messages.RemoveAt(0);

            ConsoleRenderer.Render(_mapManager, _ui, _messages, _player, _showInventory);

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
