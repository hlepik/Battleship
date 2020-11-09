using System;

namespace GameBrain
{
    public class BoatSizes
    {
        public static int CalculateBoats(BattleShip game)
        {
            var size = game.Height * game.Width / 5;
            var biggestBoat = 0;

            if (size <= 7)
            {
                biggestBoat = 2;
            }
            if (size > 7 && size <= 15)
            {
                biggestBoat = 3;
            }
            if (size > 15 && size <= 27)
            {
                biggestBoat = 4;
            }
            if (size > 27 && size <= 45)
            {
                biggestBoat = 5;
            }
            if (size > 45 && size <= 70)
            {
                biggestBoat = 6;
            }
            if (size > 70 && size <= 100)
            {
                biggestBoat = 7;
            }
            if (size > 100 )
            {
                biggestBoat = 8;
            }

            return biggestBoat;
        }

    }
}