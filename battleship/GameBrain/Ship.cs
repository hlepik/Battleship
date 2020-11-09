using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace GameBrain
{
    public class Ship
    {
        public  string Name { get; set; } = null!;
        public int Width { get; set; }
        public int ShipId { get; set; }
        public int LifeCount { get; set; }
        public bool IsSunken { get; set; }
        public string Direction { get; set; } = null!;

        public Ship(string name, int size, string direction, int shipId)
        {
            Name = name;
            Width = size;
            Direction = direction;
            LifeCount = size;
            ShipId = shipId;
        }

        public Ship()
        {

        }
    }

}