namespace HexWorld
{
    /*
        Basic cell in our world.
    */
    public class Hex
    {
        private readonly int _column, _row;

        public int Row => _row;

        public int Column => _column;

        private const int ColumnKeyOffset = 32768;

        public Hex(int column, int row)
        {
            this._column = column;
            this._row = row;
        }

        public int MapKey()
        {
            return _column * ColumnKeyOffset + _row;
        }

        public static int MapKey(int column, int row)
        {
            return column * ColumnKeyOffset + row;
        }

        public static Hex operator +(Hex left, Hex right)
        {
            return new Hex(left._column + right._column,
                left._row + right._row);
        }

        public static Hex operator -(Hex left, Hex right)
        {
            return new Hex(left._column - right._column,
                left._row - right._row);
        }

        public bool IsAtTopBorder(int topBorder)
        {
            return _row == topBorder;
        }

        public bool IsAtBottomBorder(int bottomBorder)
        {
            return _row == bottomBorder;
        }

        public bool IsAtLeftBorder(int width)
        {
            return _column == -width / 2 - 1 + width % 2 - (_row + _row % 2) / 2;
        }

        public bool IsAtRightBorder(int width)
        {
            return _column == width / 2 - 1 + width % 2 - (_row + _row % 2) / 2;
        }

        public bool IsInCorner(int width, int topBorder, int bottomBorder)
        {
            return (IsAtLeftBorder(width) || IsAtRightBorder(width)) &&
                   (IsAtTopBorder(topBorder) || IsAtBottomBorder(bottomBorder));
        }

    }
}