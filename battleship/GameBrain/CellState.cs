using System;

namespace GameBrain
{
    public class CellState
    {
        public bool  Empty { get; set; } = true;
        public bool Ship { get; set; } = false;
        public bool Miss { get; set; } = false;
        public bool Bomb { get; set; } = false;
        public int ShipId { get; set; } = 0;

    }

}