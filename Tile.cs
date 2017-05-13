using System;
using HexWorld.Enums;

namespace HexWorld
{
    public class Tile
    {
        public TileTypes Type { get; set; }
        public bool IsWater { get; }

        public Tile(TileTypes type)
        {
            Type = type;
            switch (Type)
            {
                case TileTypes.Ocean:
                    IsWater = true;
                    break;
                case TileTypes.Desert:
                    IsWater = false;
                    break;
                default:
                    throw new ArgumentException("Unknown Tile Type");
            }
        }
    }
}