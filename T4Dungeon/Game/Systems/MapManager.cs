using System;
using System.Collections.Generic;
using System.Text;
using T4Dungeon.Game.Models;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Systems
{
    public class MapManager
    {
        private readonly int _width;
        private readonly int _height;
        private readonly Random _rng = new();

        public Cell[,] Grid { get; private set; }
        public Vector2Int PlayerPosition { get; set; }
        public Vector2Int ExitPosition { get; private set; }
        public int CurrentTier { get; private set; } = 1;

        public MapManager(int width, int height)
        {
            _width = width;
            _height = height;
            GenerateMap();
        }

        public void GenerateMap() 
        { 
            Grid = new Cell[_width, _height];

            for (int x =0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Grid[x, y] = new Cell(x, y);
                }
            }

            PlayerPosition = new Vector2Int(0, 0);
            Grid[0, 0].Explored = true;

            PlaceExit();
            FillCells();
        }

        public void PlaceExit()
        {
            int x = _rng.Next(_width);
            int y = _rng.Next(_height);
            ExitPosition = new Vector2Int(x, y);
            Grid[x, y].Type = CellType.Exit;
        }

        private void FillCells()
        {
            foreach(var cell in Grid)
            {
                if (cell.CellPosition.X == PlayerPosition.X && cell.CellPosition.Y == PlayerPosition.Y)
                {
                    cell.Type = CellType.Empty;
                    cell.Event = CellEventFactory.Create(CellType.Empty);
                    continue;
                }

                if (cell.Type == CellType.Exit)
                {
                    cell.Event = CellEventFactory.Create(CellType.Exit);
                    continue;
                }

                cell.Type = RollCellType();
                cell.Event = CellEventFactory.Create(cell.Type);
            }
        }

        private CellType RollCellType()
        {
            int roll = _rng.Next(100);

            if (roll < 40) return CellType.Combat;     // 40%
            if (roll < 60) return CellType.Empty;      // 20%
            if (roll < 80) return CellType.Treasure;   // 20%
            if (roll < 95) return CellType.Shop;       // 15%

            return CellType.Empty; // fallback

        }

        private void RevealAdjacent(Vector2Int pos)
        {
            for (int dx = -1; dx <= 1; dx++)
                for (int dy = -1; dy <= 1; dy++)
                {
                    int nx = pos.X + dx;
                    int ny = pos.Y + dy;

                    if (nx >= 0 && nx < _width && ny >= 0 && ny < _height)
                        Grid[nx, ny].Explored = true;
                }
        }

        private Cell GetCell(Vector2Int pos)
        {
            return Grid[pos.X, pos.Y];
        }
    }
}
