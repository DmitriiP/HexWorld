using System;
using System.Collections.Generic;
using System.Linq;
using HexWorld.Enums;

namespace HexWorld
{
    public class MapGenerator
    {
        private Random _random;

        public MapGenerator()
        {
            _random = new Random();
        }

        public void CellularGenerator(Grid grid, double waterPercentage)
        {
            const int refine = 10;
            // Firsly randomly fill cells with desert
            foreach (var hex in grid.GetAllCells())
            {
                var roll = _random.NextDouble();
                if (roll >= waterPercentage)
                    hex.Tile = new Tile(TileTypes.Desert);
            }

            // Secondary we are using cellular automata to compare each cell to it neighbors.
            // There are several refine steps, through which, we should get good chunks of land.
            void CellularAutomata()
            {
                var visited = new Dictionary<int, bool>();
                var queue = new Queue<Hex>();
                // picking several points on the map, where we start our algorithm
                for (var i = 0; i < 4; i++)
                {
                    var row = _random.Next(grid.TopBorder, grid.BottomBorder);
                    var rowBorders = Grid.RowBorders(grid.Width, row);
                    var column = _random.Next(rowBorders.Item1, rowBorders.Item2);
                    var hex = grid.GetHexAt(column, row);
                    Console.WriteLine(hex);
                    queue.Enqueue(hex);
                }

                while (queue.Count > 0)
                {
                    var hex = queue.Dequeue();
                    if (visited.ContainsKey(hex.MapKey()))
                        continue;
                    var neighbors = grid.GetNeighbors(hex);
                    var sameTypeCount = neighbors.Count(x => x.Value != null && x.Value.Tile.Type == hex.Tile.Type);

                    var chanceToChange = 0.7;
                    if (sameTypeCount == 4)
                        chanceToChange = 0.2;
                    if (sameTypeCount >= 5)
                        chanceToChange = 0.05;
                    if (_random.NextDouble() <= chanceToChange)
                        hex.Tile.ChangeTile(hex.Tile.Type == TileTypes.Desert ? TileTypes.Ocean : TileTypes.Desert);
                    visited[hex.MapKey()] = true;
                    foreach (var neighbor in neighbors)
                    {
                        if (neighbor.Value != null && !visited.ContainsKey(neighbor.Value.MapKey()))
                            queue.Enqueue(neighbor.Value);
                    }
                }
            }

            for (var i = 0; i < refine; i++)
            {
                Console.WriteLine($"Refine run {i}");
                CellularAutomata();
            }
        }
    }
}