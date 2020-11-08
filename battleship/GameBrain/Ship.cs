using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace GameBrain
{
    public class Ship
    {
        public  string Name { get; set; } = null!;
        public int Width { get; set; }
        public List<int[]> AllPositions { get; set; } = null!;
        public int ShipId { get; set; }
        public int Count { get; set; }
        public bool IsSunken { get; set; }
        public string Direction { get; set; } = null!;

        public Ship(string name, int size, List<int[]> allPositions, string direction, int shipId)
        {
            Name = name;
            Width = size;
            Direction = direction;
            Count = size;
            AllPositions = allPositions;
            ShipId = shipId;
        }

        public Ship()
        {

        }
    }

}