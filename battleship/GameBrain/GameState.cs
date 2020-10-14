using System.ComponentModel.DataAnnotations;

namespace GameBrain
{
    public class GameState
    {
        public string PlayerFirst { get; set; } = null!;
        public string PlayerSecond { get; set; }= null!;

        public CellState[][] Board1 { get; set; } = null!;
        public CellState[][] Board2 { get; set; } = null!;

        public int Width { get; set; }
        public int Height { get; set; }

        public bool NextMoveByX { get; set; }


    }
}