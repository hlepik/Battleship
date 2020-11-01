namespace Domain
{
    public class BoardSquareState
    {
        // value from gameboat.GameBoatId
        public int? BoatId { get; set; }
        public int Bomb { get; set; } // 0 / no bomb yet here, 1..X - bomb placements in numbered order
    }
}