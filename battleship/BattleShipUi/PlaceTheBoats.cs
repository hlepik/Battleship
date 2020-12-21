using System.Drawing;
using System.Linq;
using GameBrain;
using Console = Colorful.Console;

namespace BattleShipUi
{
    public class PlaceTheBoats
    {
        public string BoatsLocation(BattleShip game, string playerName)
        {
            var place = new CanInsertBoat();
            var boat = new BoatCount();
            var insert = new InsertingBoats();
            game.CanInsert = true;

            Console.Clear();
            game.WhoWillPlaceTheShips = insert.InsertingBoat(playerName);
            if (game.WhoWillPlaceTheShips == "M")
            {
                System.Console.WriteLine($"{playerName} please insert your ships!");
            }

            var allBoats = boat.BoatsCount(game.Width, game.Height).OrderBy(x => x.Width)
                .Reverse();
            foreach (var each in allBoats)
            {
                var x = 0;
                var y = 0;
                var direction = "R";


                var board = game.GetBoard(playerName);
                BattleShipConsoleUi.DrawBoard(board);

                if (game.WhoWillPlaceTheShips == "A" || playerName == "AI")
                {

                    var random = new RandomBoats();
                    (x, y, direction) = random.RandomBoat(game, playerName, each.Width);

                }

                if (game.WhoWillPlaceTheShips == "M" && playerName != "AI")
                {

                    do
                    {
                        Console.ForegroundColor = Color.Aqua;
                        System.Console.Write($"Enter {each.Name} size={each.Width} location: ");
                        (x, y) = MoveCoordinates.GetMoveCoordinates(game);

                        if (each.Width > 1 && x < game.Width && y < game.Height )
                        {

                            do
                            {
                                System.Console.Write($"Enter {each.Width} direction R = right, D = down: ");
                                direction = Console.ReadLine().Trim().ToUpper();
                                if (direction != "R" && direction != "D")
                                {
                                    Console.ForegroundColor = Color.Maroon;
                                    System.Console.WriteLine("Invalid input!");
                                    Console.ForegroundColor = Color.Blue;
                                }


                            } while (direction != "R" && direction != "D");

                        }

                        if (x < game.Width || y < game.Height)
                        {
                            place.BoatLocationCheck(game, x, y, each.Width, direction, playerName);

                        }

                        if (game.CanInsert) continue;
                        Console.ForegroundColor = Color.Maroon;
                        System.Console.WriteLine("You can't place your ship here!");
                        Console.ForegroundColor = Color.Blue;

                    } while (!game.CanInsert);
                }

                game.InsertBoat(x, y, playerName, each.Width, direction, each.Name);
            }

            return "";


        }
    }
}