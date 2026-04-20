using T4Dungeon.Game.Events;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Models
{
    public class Cell
    {
        public Vector2Int CellPosition { get; }
        public CellType Type { get; set;  }
        public bool Explored { get; set; } = false;
        public ICellEvent Event { get; set; }

        public Cell(int x, int y)
        {
            CellPosition = new Vector2Int(x, y);
            Type = CellType.Empty;
            Explored = false;
        }
    }
}
