using Domain;

namespace GameBrain
{
    public class Board
    {
        public static CellState[,] BoardCells = null!;
        public static int BoatId { get; set; }
        public CellState[,] Board1 = null!;
        public CellState[,] Board2 = null!;

        public int ShipId { get; set; }
        public static int Count { get; set; }
        public bool IsSunken { get; set; }

        public string Direction { get; set; } = null!;

        public CellState[,] getBoard()
        {
            return Board1;
        }

        public Board(int size,  string direction, CellState[,] board, int shipId)
        {
            Direction = direction;
            Count = size;
            Board1 = board;
            ShipId = shipId;
        }

        public Board()
        {

        }
    }
}