using T4Dungeon.Game.Models;
using T4Dungeon.Game.Systems;
using T4Dungeon.Game.Utils;

namespace T4Dungeon.Game.Core
{
    public class GameEngine
    {
        private GameState _state;

        private Player _player;
        private MapManager _mapManager;
        private UIContext _ui;

        private string _message = "";

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
                        ConsoleRenderer.Render(_mapManager, _ui, _message);
                        _message = "";
                        HandleInput();
                        break;
                }
            }

            //ConsoleRenderer.Render(_mapManager);

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
        }

        //private void RunMainGameLoop()
        //{
        //    ConsoleRenderer.Render(_mapManager, _ui);
        //}

        private void SetMainMenu()
        {
            _ui = new UIContext();

            _ui.Options = new List<MenuOption>
            {
                new MenuOption { Text = "Move", Action = SetMoveMenu },
                new MenuOption { Text = "Open Inventory", Action = null, IsImplemented = false },
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

        private void MovePlayer(int dx, int dy)
        {
            var pos = _mapManager.PlayerPosition;

            var newPos = new Vector2Int(pos.X + dx, pos.Y + dy);
            if (!ValidateMove(dx, dy))
                return;

            _mapManager.PlayerPosition = newPos;
        }

        private bool ValidateMove(int dx, int dy)
        {
            var pos = _mapManager.PlayerPosition;
            var newPos = new Vector2Int(pos.X + dx, pos.Y + dy);
            if (newPos.X < 0 || newPos.X >= _mapManager.Grid.GetLength(0) ||
                newPos.Y < 0 || newPos.Y >= _mapManager.Grid.GetLength(1))
            {
                _message = "Cannot move outside map.";
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
                    _message = "Invalid option.";
                    break; // ← EXIT so screen can re-render
                }

                var option = _ui.Options[index];

                if (!option.IsImplemented)
                {
                    _message = "Option not implemented.";
                    break; // ← EXIT
                }

                option.Action?.Invoke();
                break;
            }
        }

    }
}
