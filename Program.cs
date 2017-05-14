using System;

namespace HexWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            const int width = 50;
            const int height = 80;
            var grid = new Grid(width, height);
            var mapGenerator = new MapGenerator();
            mapGenerator.CellularGenerator(grid, 0.6);
            for (var i = 0; i < 10; i++)
                mapGenerator.GenerateMountains(grid);
            const int row = 0;
            var center = grid.GetHexAt(-4, row);
            var neighbors = grid.GetNeighbors(center);
            foreach (var neighbor in neighbors)
            {
                Console.WriteLine(neighbor.Value != null
                    ? $"Neighbor {neighbor.Key} {neighbor.Value}"
                    : $"Neighbor {neighbor.Key} missing!");
            }
            Console.Write(grid);
            Console.WriteLine("Hello World!");
        }
    }
}