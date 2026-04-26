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

        if (player != null)
        {
            DrawPlayerStats(player);
            DrawInventory(player, showInventory);
        }
        else
        {
            DrawTitleLogo();
        }

        DrawLeftMenu(ui, map, isCombat, enemy);

        if (isCombat)
            DrawCombatArena();
        else if (map == null && player != null)
            DrawShopHeader();
        else if (map != null)
            DrawMap(map);
        else
            DrawStartScreenArt();

        DrawBottomText(messages);
    }

    private static void DrawTitleLogo()
    {
        // Matches the website hero — magenta/bold logo, cyan subtitle
        Console.WriteLine($"{TextColor.Magenta}{TextColor.Bold}");
        Console.WriteLine("\t\t████████╗██╗  ██╗    ██████╗ ");
        Console.WriteLine("\t\t╚══██╔══╝██║  ██║    ██╔══██╗");
        Console.WriteLine("\t\t   ██║   ███████║    ██║  ██║");
        Console.WriteLine("\t\t   ██║   ╚════██║    ██║  ██║");
        Console.WriteLine("\t\t   ██║        ██║    ██████╔╝");
        Console.WriteLine("\t\t   ╚═╝        ╚═╝    ╚═════╝ ");
        Console.WriteLine($"{TextColor.Reset}{TextColor.Cyan}\t\t      DUNGEON CRAWLER");
        Console.WriteLine($"{TextColor.Gray}~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~{TextColor.Reset}");
    }

    private static void DrawStartScreenArt()
    {
        for (int i = 0; i < 3; i++) Console.WriteLine();
        Console.WriteLine($"\t\t      {TextColor.Gray}[ {TextColor.Green}Press a key to begin{TextColor.Gray} ]{TextColor.Reset}");
        for (int i = 0; i < 7; i++) Console.WriteLine();
    }

    private static void DrawTopBar()
    {
        Console.WriteLine($"{TextColor.Gray}==============================================================================={TextColor.Reset}");
    }

    private static void DrawCombatArena()
    {
        // Matches website combat section — red attacker vs green defender
        for (int i = 0; i < 5; i++) Console.WriteLine();
        Console.WriteLine($"\t\t\t      {TextColor.Red}(╯°□°)╯{TextColor.Reset}  VS  {TextColor.Green}(O_O){TextColor.Reset}");
        Console.WriteLine($"\t\t\t      {TextColor.Yellow}{TextColor.Bold}[ COMBAT MODE ]{TextColor.Reset}");
        for (int i = 0; i < 5; i++) Console.WriteLine();
        Console.WriteLine($"\t\t\t\t{TextColor.Gray}---------------------{TextColor.Reset}");
    }

    private static void DrawShopHeader()
    {
        Console.WriteLine($"\n\n\t   {TextColor.Yellow}{TextColor.Bold}[ MERCHANT'S CARAVAN ]{TextColor.Reset}");
        Console.WriteLine($"\t   {TextColor.Gray}\"Got some rare things on sale, stranger!\"{TextColor.Reset}");
        Console.WriteLine($"\t   {TextColor.Gray}________________________________________{TextColor.Reset}");
    }

    private static void DrawPlayerStats(Player player)
    {
        // HP color shifts green → yellow → red based on health percentage
        double hpPercent = (double)player.HP / player.MaxHP;
        string hpColor = hpPercent > 0.6 ? TextColor.Green : hpPercent > 0.3 ? TextColor.Yellow : TextColor.Red;

        Console.Write($"{TextColor.Cyan}HP: {hpColor}{TextColor.Bold}{player.HP}/{player.MaxHP}{TextColor.Reset}");
        Console.Write($"  {TextColor.Gray}|{TextColor.Reset}  ATK: {TextColor.White}{player.Attack}{TextColor.Reset}");
        Console.Write($"  {TextColor.Gray}|{TextColor.Reset}  DEF: {TextColor.White}{player.Defense}{TextColor.Reset}");
        Console.Write($"  {TextColor.Gray}|{TextColor.Reset}  ");
        Console.WriteLine($"{TextColor.Yellow}{TextColor.Bold}GOLD: {player.Gold}{TextColor.Reset}");
        Console.WriteLine($"{TextColor.Gray}~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~{TextColor.Reset}");
    }

    private static void DrawLeftMenu(UIContext ui, MapManager? map, bool isCombat, Enemy? currentEnemy = null)
    {
        for (int i = 0; i < ui.Options.Count; i++)
        {
            var opt = ui.Options[i];
            // Unimplemented options are grayed out like the website's dim text
            string numColor  = opt.IsImplemented ? TextColor.Yellow : TextColor.Gray;
            string textColor = opt.IsImplemented ? TextColor.White  : TextColor.Gray;
            Console.WriteLine($"  {numColor}{i + 1}.{TextColor.Reset} {textColor}{opt.Text}{TextColor.Reset}");
        }

        Console.WriteLine();

        if (isCombat && currentEnemy != null)
        {
            // Enemy HP also shifts color like player HP
            double enemyHpPercent = (double)currentEnemy.HP / currentEnemy.MaxHp;
            string ehpColor = enemyHpPercent > 0.6 ? TextColor.Green : enemyHpPercent > 0.3 ? TextColor.Yellow : TextColor.Red;

            Console.WriteLine(
                $"\t\t\t\t{TextColor.Red}{TextColor.Bold}{currentEnemy.Name.ToUpper()}{TextColor.Reset}" +
                $"  {TextColor.Gray}|{TextColor.Reset}  HP: {ehpColor}{TextColor.Bold}{currentEnemy.HP}/{currentEnemy.MaxHp}{TextColor.Reset}"
            );
        }
        else if (map != null)
        {
            Console.WriteLine(
                $"\t\t\t\t{TextColor.Gray}Tier: {TextColor.Cyan}{map.CurrentTier}{TextColor.Reset}" +
                $"  {TextColor.Gray}| Cell: {TextColor.Cyan}{map.PlayerPosition.X}-{map.PlayerPosition.Y}{TextColor.Reset}"
            );
        }
    }

    private static void DrawInventory(Player player, bool showInventory)
    {
        if (!showInventory) return;
        Console.WriteLine($"  {TextColor.Yellow}{TextColor.Bold}- INVENTORY{TextColor.Reset}");
        Console.WriteLine($"  {TextColor.Gray}NAME\t| AMOUNT | DESCRIPTION{TextColor.Reset}");
    }

    private static void DrawMap(MapManager map)
    {
        // Matches website map section colors exactly
        for (int y = 0; y < map.Grid.GetLength(1); y++)
        {
            Console.Write($"\t\t\t\t{TextColor.Gray}|{TextColor.Reset}");

            for (int x = 0; x < map.Grid.GetLength(0); x++)
            {
                if (map.PlayerPosition.X == x && map.PlayerPosition.Y == y)
                {
                    Console.Write($"{TextColor.Green}{TextColor.Bold}P {TextColor.Reset}");
                    continue;
                }

                var cell = map.Grid[x, y];

                if (IsDebugMode && cell.Type == CellType.Exit)
                {
                    Console.Write($"{TextColor.Yellow}E {TextColor.Reset}");
                    continue;
                }

                if (!cell.Explored)
                {
                    Console.Write($"{TextColor.Gray}# {TextColor.Reset}");
                }
                else
                {
                    Console.Write(GetColoredChar(cell.Type) + " ");
                }
            }

            Console.WriteLine($"{TextColor.Gray}|{TextColor.Reset}");
        }

        Console.WriteLine($"\t\t\t\t{TextColor.Gray}---------------------{TextColor.Reset}");
    }

    private static string GetColoredChar(CellType type)
    {
        // Colors match the website legend exactly
        return type switch
        {
            CellType.Empty    => $"{TextColor.Gray}.{TextColor.Reset}",
            CellType.Combat   => $"{TextColor.Red}C{TextColor.Reset}",
            CellType.Treasure => $"{TextColor.Yellow}T{TextColor.Reset}",
            CellType.Shop     => $"{TextColor.Cyan}S{TextColor.Reset}",
            CellType.Exit     => $"{TextColor.Green}E{TextColor.Reset}",
            _                 => $"{TextColor.Gray}?{TextColor.Reset}"
        };
    }

    private static void DrawBottomText(List<string> messages)
    {
        Console.WriteLine($"{TextColor.Gray}__________________________________________________{TextColor.Reset}");

        foreach (var msg in messages.TakeLast(4))
        {
            Console.WriteLine($"  {TextColor.Gray}-{TextColor.Reset} {msg}");
        }

        for (int i = messages.Count; i < 4; i++)
            Console.WriteLine($"  {TextColor.Gray}-{TextColor.Reset}");
    }
}