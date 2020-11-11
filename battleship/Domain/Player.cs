using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain;

namespace Domain
{
    public class Player
    {
        public int PlayerId { get; set; }
        [MaxLength(128)]
        public string Name { get; set; } = null!;

        public EPlayerType EPlayerType { get; set; }

        public Game Game { get; set; } = null!;
        public int? GameId { get; set; }

        public ICollection<PlayerBoardState> PlayerBoardStates { get; set; } = null!;
        public ICollection<GameBoat> GameBoats { get; set; } = null!;


    }
}