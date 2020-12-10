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
        public bool Inserted { get; set; }

        public Ship(string name, int size, string direction, int shipId, bool isSunken)
        {
            Name = name;
            Width = size;
            Direction = direction;
            LifeCount = size;
            ShipId = shipId;
            IsSunken = isSunken;
        }
        public Ship(string name, int size, int shipId, bool inserted)
        {
            Name = name;
            Width = size;
            ShipId = shipId;
            Inserted = inserted;
        }
        public Ship()
        {

        }
    }

}