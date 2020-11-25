using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameBoat
    {
        public int GameBoatId { get; set; }

        [Range(1, int.MaxValue)]
        public int Size { get; set; }

        [MaxLength(32)]
        public string Name { get; set; } = null!;
        public int LifeCount { get; set; }

        public string Direction { get; set; } = null!;
        public int ShipId { get; set; }

        public bool IsSunken { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;


    }
}