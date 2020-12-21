using System;

namespace GameBrain
{
    public class RandomBoats
    {
        public (int x, int y, string Direction) RandomBoat(BattleShip game, string player, int size)
        {
            var boat = new CanInsertBoat();
            var x = 0;
            var y = 0;
            var direction = "";
            do
            {
                Random rand = new Random();
                x = rand.Next(0, game.Width);
                y = rand.Next(0, game.Height);

                var num = rand.Next(1, 3);
                if (num == 1)
                {
                    direction = "R";
                }
                else
                {
                    direction = "D";
                }

                boat.BoatLocationCheck(game, x, y, size, direction!, player);

            }while (!game.CanInsert);

            return (x, y, direction);
        }

    }
}