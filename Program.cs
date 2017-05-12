using System;

namespace HexWorld
{
    class Program
    {
        static void Main(string[] args)
        {
//            var grid = new Grid(1024, 1024);
//            Console.WriteLine(Grid.RowBorders(1024, -1));
//            Console.WriteLine(Grid.RowBorders(1024, 0));
//            Console.WriteLine(Grid.RowBorders(1024, 1));
//            var center = grid.GetHexAt(-513, 0);
            const int width = 9;
            const int row = -3;
            var grid = new Grid(width, 6);
            Console.WriteLine(Grid.RowBorders(width, row - 1));
            Console.WriteLine(Grid.RowBorders(width, row));
            Console.WriteLine(Grid.RowBorders(width, row + 1));
            var center = grid.GetHexAt(-3, row);
            var neighbors = grid.GetNeighbors(center);
            grid.GetNeighbors(center);
            grid.GetNeighbors(center);
            foreach (var neighbor in neighbors)
            {
                Console.WriteLine(neighbor.Value != null
                    ? $"Neighbor {neighbor.Key} {neighbor.Value.Column} {neighbor.Value.Row}"
                    : $"Neighbor {neighbor.Key} missing!");
            }
            Console.WriteLine("Hello World!");
        }
    }
}