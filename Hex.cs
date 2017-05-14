using HexWorld.Enums;

namespace HexWorld
{
    /*
        Basic cell in our world.
    */
    public class Hex
    {
        public int Row { get; }

        public int Column { get; }

        public Tile Tile { get; set; }

        private const int ColumnKeyOffset = 32768;

        public Hex(int column, int row, Tile tile=null)
        {
            Column = column;
            Row = row;
            Tile = tile ?? new Tile(TileTypes.Ocean);
        }

        public override string ToString()
        {
            return $"[{MapKey()}] Hex at ({Column},{Row}), has type of {Tile.Type}";
        }

        public int MapKey()
        {
            return Column * ColumnKeyOffset + Row;
        }

        public static int MapKey(int column, int row)
        {
            return column * ColumnKeyOffset + row;
        }

        public static Hex operator +(Hex left, Hex right)
        {
            return new Hex(left.Column + right.Column,
                left.Row + right.Row);
        }

        public static Hex operator -(Hex left, Hex right)
        {
            return new Hex(left.Column - right.Column,
                left.Row - right.Row);
        }

        public bool IsAtTopBorder(int topBorder)
        {
            return Row == topBorder;
        }

        public bool IsAtBottomBorder(int bottomBorder)
        {
            return Row == bottomBorder;
        }

        public bool IsAtLeftBorder(int width)
        {
            return Column == -width / 2 - 1 + width % 2 - (Row + Row % 2) / 2;
        }

        public bool IsAtRightBorder(int width)
        {
            return Column == width / 2 - 1 + width % 2 - (Row + Row % 2) / 2;
        }

        public bool IsInCorner(int width, int topBorder, int bottomBorder)
        {
            return (IsAtLeftBorder(width) || IsAtRightBorder(width)) &&
                   (IsAtTopBorder(topBorder) || IsAtBottomBorder(bottomBorder));
        }

    }
}