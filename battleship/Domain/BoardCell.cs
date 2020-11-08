namespace Domain
{
    public class BoardCells
    {
        public static int BoardCellId { get; set; }
        public static BoardCells[,] CellState = null!;
        public static BoardCells Bomb = null!;
    }
}