using T4Dungeon.Game.Models;

public interface ICellEvent
{
    string Execute(Player player, Cell cell);
}