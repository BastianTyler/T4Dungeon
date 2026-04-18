using T4Dungeon.Game.Models;
using T4Dungeon.Game.Systems;

namespace T4Dungeon.Game.Core
{
    public class GameEngine
    {
        private bool _running;
        private readonly EventSystem _eventSystem;
        private readonly Player _player;
        private MapManager _mapManager;
        private Vector2Int  _playerPosition;

        public GameEngine()
        {
            _eventSystem = new EventSystem();
            _player = new Player();
            _mapManager = new MapManager(10, 8);
            _playerPosition = _mapManager.PlayerPosition;
        }

        public void Run()
        {
            _running = true;

            Console.WriteLine("Welcome to T4Dungeon!");

            while (_running)
            {

                if(_player.IsDead)
                {
                    Console.WriteLine("You have died. Game Over.");
                    _running = false;
                }
            }

            Console.WriteLine("Game has ended. Thanks for playing!");
        }
    }
}
