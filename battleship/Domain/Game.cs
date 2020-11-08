using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Game
    {

        public int GameId { get; set; }

        public int GameOptionId { get; set; }
        public GameOption GameOption { get; set; } = null!;

        // see halb, peaks olema piirang peal
        [MaxLength(128)]
        public string Description { get; set; } = DateTime.Now.ToLongDateString();

        public int PlayerAId { get; set; }
        public Player PlayerA { get; set; } = null!;

        public int PlayerBId { get; set; }
        public Player PlayerB { get; set; } = null!;

    }
}