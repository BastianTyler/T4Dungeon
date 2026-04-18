using T4Dungeon.Game.Core;
using T4Dungeon.Game.Systems;
using T4Dungeon.Tests.MapTests;

namespace T4Dungeon.Game.Core
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var engine = new GameEngine();
            //engine.Run();

            var map = new MapManager(10, 8);

            MapDebugPrinter.Print(map);

            Console.ReadKey();
        }
    }           
}


