﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Boat
    {
        public int BoatId { get; set; }

        [Range(1, int.MaxValue)]
        public int Size { get; set; }

        public bool Inserted { get; set; }

        [MaxLength(32)]
        public string Name { get; set; } = null!;
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;


        // public ICollection<GameOptionBoat> GameOptionBoats { get; set; } = null!;

    }
}