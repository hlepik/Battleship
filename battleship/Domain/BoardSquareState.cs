using System.Collections;
using System.Collections.Generic;

namespace Domain
{
    public class BoardSquareState
    {
        // value from gameboat.GameBoatId
        public int BoatId { get; set; }
        public bool Bomb { get; set; }

        public ICollection<BoardCells> BoardCells { get; set; } = null!;

    }
}