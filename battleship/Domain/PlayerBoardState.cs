using System;
using Domain;

namespace Domain
{
    public class PlayerBoardState
    {
        public int PlayerBoardStateId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;

        // serialized to json
        public string BoardState { get; set; } = null!;

    }
}