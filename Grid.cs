using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HexWorld.Enums;

namespace HexWorld
{
    public class Grid
    {
        private readonly Dictionary<int, Hex> _map;
        private readonly Dictionary<int, Dictionary<HexNeighbors, Hex>> _neighbors;
        public int Width { get; }
        public int TopBorder { get; }
        public int BottomBorder { get; }
        private readonly bool _wrapX;
        private readonly bool _wrapY;

        public Grid(int width, int height, bool wrapX = true, bool wrapY = false)
        {
            Width = width;
            _wrapX = wrapX;
            _wrapY = wrapY;
            if (_wrapY)
                throw new NotImplementedException("Y-Axis Wraps are not implemented yet.");
            TopBorder = -height / 2;
            BottomBorder = (height + height % 2) / 2 - 1;
            _map = new Dictionary<int, Hex>(height * width);
            _neighbors = new Dictionary<int, Dictionary<HexNeighbors, Hex>>(height * width);
            for (var y = TopBorder; y <= BottomBorder; y++)
            {
                var rowBorders = RowBorders(width, y);
                var leftBorder = rowBorders.Item1;
                var rightBorder = rowBorders.Item2;

                for (var x = leftBorder; x <= rightBorder; x++)
                {
                    var hex = new Hex(x, y);
                    _map.Add(hex.MapKey(), hex);
                }
            }
        }

        public static Tuple<int, int> RowBorders(int width, int rowNumber)
        {
            var rowOffset = (rowNumber + Math.Abs(rowNumber % 2)) / 2;
            var leftBorder = -width / 2 - 1 + width % 2 - rowOffset;
            var rightBorder = width / 2 - 1 + width % 2 - rowOffset;
            return new Tuple<int, int>(leftBorder, rightBorder);
        }

        public Hex GetHexAt(Hex coordinates)
        {
            var key = coordinates.MapKey();
            return _map.ContainsKey(key) ? _map[key] : null;
        }

        public Hex GetHexAt(int column, int row)
        {
            var key = Hex.MapKey(column, row);
            return _map.ContainsKey(key) ? _map[key] : null;
        }

        public IEnumerable<Hex> GetAllCells()
        {
            return _map.Select(hex => hex.Value);
        }

        public Hex GetRandomHex(Random random=null)
        {
            if (random == null)
                random = new Random();
            var row = random.Next(TopBorder, BottomBorder);
            var rowBorders = RowBorders(Width, row);
            var column = random.Next(rowBorders.Item1, rowBorders.Item2);
            var hex = GetHexAt(column, row);
            return hex;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            for (var y = TopBorder; y <= BottomBorder; y++)
            {
                if (y % 2 == 0)
                {
                    result.Append("  ");
                }
                var rowBorders = RowBorders(Width, y);
                var leftBorder = rowBorders.Item1;
                var rightBorder = rowBorders.Item2;

                for (var x = leftBorder; x <= rightBorder; x++)
                {
                    result.Append(GetHexAt(x, y).Tile + " ");
                }
                result.AppendLine();
            }
            return result.ToString();
        }

        public Dictionary<HexNeighbors, Hex> GetNeighbors(Hex hex)
        {
            if (_neighbors.ContainsKey(hex.MapKey()))
                return _neighbors[hex.MapKey()];
            var result = new Dictionary<HexNeighbors, Hex>();
            var directions = new Dictionary<HexNeighbors, Hex>
            {
                [HexNeighbors.NorthEast] = new Hex(1, -1),
                [HexNeighbors.East] = new Hex(1, 0),
                [HexNeighbors.SouthEast] = new Hex(0, 1),
                [HexNeighbors.SouthWest] = new Hex(-1, 1),
                [HexNeighbors.West] = new Hex(-1, 0),
                [HexNeighbors.NorthWest] = new Hex(0, -1),
            };
            foreach (var direction in directions)
            {
                result[direction.Key] = GetHexAt(hex + direction.Value);
            }

            var isEvenRow = hex.Row % 2 == 0;
            var rowBorders = RowBorders(Width, hex.Row);
            var leftBorder = rowBorders.Item1;
            var rightBorder = rowBorders.Item2;
            if (_wrapX && _wrapY && hex.IsInCorner(Width, TopBorder, BottomBorder))
            {
                // TODO this will be ugly, will leave it as it is for now.
                throw new NotImplementedException();
            }
            else if (_wrapX && hex.Column == leftBorder)
            {
                var west = _map[Hex.MapKey(rightBorder, hex.Row)];
                result[HexNeighbors.West] = west;
                if (!isEvenRow)
                {
                    result[HexNeighbors.NorthWest] = GetHexAt(west + directions[HexNeighbors.NorthEast]);
                    result[HexNeighbors.SouthWest] = GetHexAt(west + directions[HexNeighbors.SouthEast]);
                }
            }
            else if (_wrapX && hex.Column == rightBorder)
            {
                var east = _map[Hex.MapKey(leftBorder, hex.Row)];
                result[HexNeighbors.East] = east;
                if (isEvenRow)
                {
                    result[HexNeighbors.NorthEast] = GetHexAt(east + directions[HexNeighbors.NorthWest]);
                    result[HexNeighbors.SouthEast] = GetHexAt(east + directions[HexNeighbors.SouthWest]);
                }
            }
            else if (_wrapY)
            {
                // TODO, for the future generations
                throw new NotImplementedException();
            }
            _neighbors.Add(hex.MapKey(), result);
            return result;
        }

        /// <summary>
        /// Gets max available area of hexes, that comply to the given criteria.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<Hex> GetArea(Hex start, Func<Hex, bool> criteria)
        {
            var result = new List<Hex>();

            void Recursion(Hex hex)
            {
                if (!criteria(hex) || result.Contains(hex)) return;
                result.Add(hex);
                foreach (var neighbor in GetNeighbors(hex).Values)
                {
                    Recursion(neighbor);
                }
            }

            Recursion(start);

            return result;
        }
    }
}