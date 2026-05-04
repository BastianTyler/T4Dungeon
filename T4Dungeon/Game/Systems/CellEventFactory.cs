using T4Dungeon.Game.Events;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Systems
{
    public static class CellEventFactory
    {
        public static ICellEvent Create(CellType type)
        {
            return type switch
            {
                CellType.Empty => new EmptyEvent(),
                CellType.Combat => new CombatEvent(),
                CellType.Treasure => new TreasureEvent(),
                CellType.Shop => new ShopEvent(),
                CellType.Exit => new ExitEvent(),
                _ => new EmptyEvent()
            };
        }
    }
}