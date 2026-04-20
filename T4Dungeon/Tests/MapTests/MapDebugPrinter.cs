using System;
using System.Collections.Generic;
using System.Text;
using T4Dungeon.Game.Systems;

namespace T4Dungeon.Tests.MapTests
{
    public static class MapDebugPrinter
    {
        public static void Print(MapManager map)
        {
            Console.WriteLine("=== MAP DEBUG ===");
            
            for(int x=0; x< map.Grid.GetLength(0); x++)
            {
                for (int y = 0; y < map.Grid.GetLength(1); y++)
                {
                    var cell = map.Grid[x, y];

                    Console.WriteLine($"Index[{x}, {y}] | Pos({cell.CellPosition.X},{cell.CellPosition.Y}) | Type: {cell.Type}");
                }
                Console.WriteLine("====================");
            }
        }
    }
}
