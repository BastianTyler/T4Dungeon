using T4Dungeon.Game.Models;
using T4Dungeon.Game.Systems;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Utils;

public static class ConsoleRenderer
{
    public static void Render(MapManager map, UIContext ui, List<string> messages, Player player, bool showInventory)
    {
        Console.Clear();

        DrawTopBar();
        DrawPlayerStats(player);
        DrawInventory(player, showInventory);
        DrawLeftMenu(ui, map);
        DrawMap(map);
        DrawBottomText(messages);
    }

    private static void DrawTopBar()
    {
        Console.WriteLine("===============================================================================");
    }

    private static void DrawPlayerStats(Player player)
    {
        Console.WriteLine($"HP: {player.HP} | Attack: {player.Attack} | Defense: {player.Defense}");
        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
    }

    private static void DrawLeftMenu(UIContext ui, MapManager map)
    {
        for (int i = 0; i < ui.Options.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {ui.Options[i].Text}");
        }

        Console.WriteLine($"\t\t\t\tTier: {map.CurrentTier} | Cell: {map.PlayerPosition.X}-{map.PlayerPosition.Y}");
        Console.WriteLine("\t\t\t\t---------------------");
    }

    private static void DrawInventory(Player player, bool showInventory)
    {
        if(!showInventory) return;

        Console.WriteLine("- INVENTORY");
        Console.WriteLine("\t NAME \t| AMOUNT | DESCRIPTION");

        //int index = 1;

        //foreach (var item in player.Inventory.Items)
        //{
        //    var def = ItemDatabase.Items[item.ItemId];

        //    Console.WriteLine($"{index}. {def.Name}\t|\t{item.Amount}\t|\t{def.Description}");
        //    index++;
        //}

        //Console.WriteLine("______");
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

        // CHANGE: show last few messages instead of single string
        foreach (var msg in messages.TakeLast(4))
        {
            Console.WriteLine($"- {msg}");
        }

        // pad if needed (keeps layout stable)
        for (int i = messages.Count; i < 4; i++)
            Console.WriteLine("-");

        Console.WriteLine("===============================================================================");
    }
}