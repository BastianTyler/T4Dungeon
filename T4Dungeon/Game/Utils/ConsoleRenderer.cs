using T4Dungeon.Game.Models;
using T4Dungeon.Game.Systems;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Utils;

public static class ConsoleRenderer
{

    public static bool IsDebugMode = true;
    public static void Render(MapManager map, UIContext ui, List<string> messages, Player player, bool showInventory, bool isCombat, Enemy? enemy = null)
    {
        Console.Clear();

        DrawTopBar();
        DrawPlayerStats(player);
        DrawInventory(player, showInventory);
        DrawLeftMenu(ui, map, isCombat, enemy);
        if (isCombat)
        {
            DrawCombatArena();
        }
        else
        {
            DrawMap(map);
        }
        DrawBottomText(messages);
    }

    private static void DrawTopBar()
    {
        Console.WriteLine("===============================================================================");
    }
    private static void DrawCombatArena()
    {
        // We print 10 empty lines (matching your 10x10 map height) 
        // to keep the "BottomText" in the same place.
        for (int i = 0; i < 5; i++) Console.WriteLine();

        Console.WriteLine("\t\t\t      (╯°□°)╯  VS  (O_O)");
        Console.WriteLine("\t\t\t      [ COMBAT MODE ]");

        for (int i = 0; i < 5; i++) Console.WriteLine();

        Console.WriteLine("\t\t\t\t---------------------");
    }
    private static void DrawPlayerStats(Player player)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"HP: {player.HP}/{player.MaxHP} ");
        Console.ResetColor();

        Console.Write($"| ATK: {player.Attack} | DEF: {player.Defense} | ");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"GOLD: {player.Gold}");
        Console.ResetColor();

        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
    }

    private static void DrawLeftMenu(UIContext ui, MapManager map, bool isCombat, Enemy? currentEnemy = null)
    {
        for (int i = 0; i < ui.Options.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {ui.Options[i].Text}");
        }

        Console.WriteLine("\t\t\t\t");

        if (isCombat && currentEnemy != null)
        {
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\t\t\t\tEnemy: {currentEnemy.Name.ToUpper()} | HP: {currentEnemy.HP}");
            Console.ResetColor();
        }
        else
        {
            
            Console.WriteLine($"\t\t\t\tTier: {map.CurrentTier} | Cell: {map.PlayerPosition.X}-{map.PlayerPosition.Y}");
        }

        Console.WriteLine("\t\t\t\t---------------------");
    }

    private static void DrawInventory(Player player, bool showInventory)
    {
        if(!showInventory) return;

        Console.WriteLine("- INVENTORY");
        Console.WriteLine("\t NAME \t| AMOUNT | DESCRIPTION");
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

                // --- DEBUG FEATURE: GOLDEN EXIT ---
                if (IsDebugMode && cell.Type == CellType.Exit)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("E ");
                    Console.ResetColor();
                    continue;
                }

                if (!cell.Explored)
                {
                    Console.Write("# ");
                }
                else
                {
                    Console.Write(GetChar(cell.Type) + " ");
                }
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

    private static void DrawBottomText(List<string> messages)
    {
        Console.WriteLine("_________________________________________________");

        foreach (var msg in messages.TakeLast(4))
        {
            Console.WriteLine($"- {msg}");
        }

        for (int i = messages.Count; i < 4; i++)
            Console.WriteLine("-");
    }
}