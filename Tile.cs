using System;
using HexWorld.Enums;

namespace HexWorld
{
    public class Tile
    {
        public TileTypes Type { get; set; }
        public bool IsWater { get; set; }
        public string Printable { get; set; }

        public Tile(TileTypes type)
        {
            ChangeTile(type);
        }

        public void ChangeTile(TileTypes type)
        {
            Type = type;
            switch (Type)
            {
                case TileTypes.Ocean:
                    IsWater = true;
//                    Printable = "▓▓▓";
                    Printable = "   ";
                    break;
                case TileTypes.Desert:
                    IsWater = false;
                    Printable = "░░░";
                    break;
                case TileTypes.Mountain:
                    IsWater = false;
                    Printable = "╔╬╗";
                    break;
                case TileTypes.Hill:
                    IsWater = false;
                    Printable = "▅▂▅";
                    break;
                case TileTypes.Grassland:
                    IsWater = false;
                    Printable = "▒▒▒";
                    break;
                case TileTypes.Steppe:
                    IsWater = false;
                    Printable = "░▒░";
                    break;
                case TileTypes.Tundra:
                    IsWater = false;
                    Printable = "***";
                    break;
                default:
                    throw new ArgumentException("Unknown Tile Type");
            }
        }

        public override string ToString()
        {
            return Printable;
        }
    }
}