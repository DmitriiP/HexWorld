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
                    var hex = grid.GetRandomHex(_random);
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

        /// <summary>
        /// Generate mountain ranges and also some hills.
        /// Algorithm simulates borders between tectonic plates.
        /// It does so by doing a random curve between two random cells.
        /// Along the way either mountains, hills or nothing is raised.
        /// Process repeats several times
        /// </summary>
        /// <param name="grid">our map goes here</param>
        public void GenerateMountains(Grid grid)
        {
            var startCell = grid.GetRandomHex(_random);
            var endCell = grid.GetRandomHex(_random);


            var range = new List<int>();
            var current = startCell;
            while (current != endCell)
            {
                // TODO check previous cells in range
                range.Add(current.MapKey());
                if (current.Tile.Type == TileTypes.Desert)
                {
                    var chance = _random.NextDouble();
                    if (chance <= 0.2)
                        current.Tile.ChangeTile(TileTypes.Mountain);
                    else if(chance <= 0.6 )
                        current.Tile.ChangeTile(TileTypes.Hill);
                }
                var smallestDistance = int.MaxValue;
                var distances = new Dictionary<HexNeighbors, int>();
                var neighbors = grid.GetNeighbors(current);
                foreach (var neighbor in neighbors)
                {
                    if (neighbor.Value == null)
                        continue;
                    var distance = neighbor.Value.DistanceTo(endCell);
                    if (distance < smallestDistance)
                        smallestDistance = distance;
                    distances[neighbor.Key] = distance;
                }
                var chances = new Dictionary<HexNeighbors, double>();
                foreach (var distance in distances)
                {
                    chances[distance.Key] = (distance.Value - smallestDistance) * 0.45 + 0.05;
                    if (chances[distance.Key] > 1)
                        chances[distance.Key] = 0.95;
                }
                if (neighbors.Values.All(p => p != null && range.Contains(p.MapKey())))
                    current = neighbors.First(p => distances[p.Key] == smallestDistance).Value;
                foreach (var distance in chances.OrderBy(d => -d.Value))
                {
                    var chance = _random.NextDouble();
                    if (chance <= distance.Value || range.Contains(neighbors[distance.Key].MapKey()))
                        continue;
                    current = neighbors[distance.Key];
                    break;
                }
            }
        }
    }
}