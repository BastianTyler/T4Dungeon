using T4Dungeon.Game.Systems;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Utils;

public static class ConsoleRenderer
{
    public static void Render(MapManager map)
    {
        Console.Clear();

        DrawTopBar(map);
        DrawLeftMenu();
        DrawMap(map);
        DrawBottomText();
    }

    private static void DrawTopBar(MapManager map)
    {
        Console.WriteLine("===============================================================================");
        Console.WriteLine($"Tier: {map.CurrentTier} | Cell: {map.PlayerPosition.X}-{map.PlayerPosition.Y}");
    }

    private static void DrawLeftMenu()
    {
        Console.WriteLine("\t\t\t\t---------------------");
        Console.WriteLine("~ Move\t\t\t\t\t|");
        Console.WriteLine("~ Open Inventory\t\t\t|");
        Console.WriteLine("~ Interact\t\t\t\t|");
        Console.WriteLine("~ Quit\t\t\t\t\t|");
    }

    private static void DrawMap(MapManager map)
    {
        for (int y = 0; y < map.Grid.GetLength(1); y++)
        {
            Console.Write("\t\t\t\t|");

            for (int x = 0; x < map.Grid.GetLength(0); x++)
            {
                if (map.PlayerPosition.X == x && map.PlayerPosition.Y == y)
                {
                    Console.Write("P ");
                    continue;
                }

                var cell = map.Grid[x, y];
                Console.Write(GetChar(cell.Type) + " ");
            }

            Console.WriteLine("|");
        }

        Console.WriteLine("\t\t\t\t---------------------");
    }

    private static char GetChar(CellType type)
    {
        return type switch
        {
            CellType.Empty => '.',
            CellType.Combat => 'C',
            CellType.Treasure => 'T',
            CellType.Shop => 'S',
            CellType.Exit => 'E',
            _ => '?'
        };
    }

    private static void DrawBottomText()
    {
        Console.WriteLine("_________________________________________________");
        Console.WriteLine("- Dialogue and text stuff occurs here");
        Console.WriteLine("-");
        Console.WriteLine("-");
        Console.WriteLine("-");
        Console.WriteLine("===============================================================================");
    }
}