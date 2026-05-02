using T4Dungeon.Game.Core;
using T4Dungeon.Game.Models;
using T4Dungeon.Game.States;
using T4Dungeon.Game.Systems;
using T4Dungeon.Tests.MapTests;

namespace T4Dungeon.Game.Core
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            //======================================
            //BEFORE REFACTOR FOR CLASSIC
            //var engine = new GameEngine();
            //engine.Run();
            //======================================

            //var map = new MapManager(10, 8);

            //MapDebugPrinter.Print(map);

            //Console.ReadKey();

            //=====================================================
            //TEMP AREA FOR REFACTOR TESTING ABOVE IS MAIN MODE

            var states = new Dictionary<GameStateType, IGameState>();

            var stateMachine = new StateMachine(states);

            var inputSystem = new InputSystem();
            var player = new Player();
            var mapManager = new MapManager(10, 10);
            var log = new GameLogSystem();
            var combatManager = new CombatManager();
            var tutorial = new TutorialManager();


            // =========================
            // CREATE STATES
            // =========================

            var startScreen = new StartScreenState(stateMachine, inputSystem);


            var combatState = new CombatState(
                stateMachine,
                inputSystem,
                log,
                mapManager,
                player
            );

            //=============================
            //CREATING COMBAT MANAGER
            //==============================

            var explorationState = new ExplorationState(
                stateMachine,
                inputSystem,
                mapManager,
                player,
                log,
                combatManager
            );

            var shopState = new ShopState(
                stateMachine,
                inputSystem,
                log,
                mapManager,
                player
            );





            // (stubs if you haven't built them fully yet)
            //IGameState combatState = new CombatState(stateMachine);
            //IGameState shopState = new ShopState(stateMachine);
            //IGameState inventoryState = new InventoryState(stateMachine);

            // =========================
            // REGISTER STATES
            // =========================

            states[GameStateType.StartScreen] = startScreen;
            states[GameStateType.Exploration] = explorationState;
            states[GameStateType.Combat] = combatState;
            states[GameStateType.Shop] = shopState;
            //states[GameStateType.Combat] = combatState;
            //states[GameStateType.Shop] = shopState;
            //states[GameStateType.Inventory] = inventoryState;

            // =========================
            // START GAME
            // =========================

            stateMachine.ChangeState(GameStateType.StartScreen);

            Maximize();
            while (true)
            {
                stateMachine.Update();
            }

        }

        public static void Maximize()
        {
            // Sets the buffer size to the largest possible for the current screen
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            // Sets the window size to the largest possible
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            // Position the window at the top-left corner
            Console.SetWindowPosition(0, 0);
        }
    }           
}


