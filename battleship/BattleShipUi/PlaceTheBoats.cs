using System;
using System.Drawing;
using System.Linq;
using Domain;
using GameBrain;
using Console = Colorful.Console;

namespace BattleShipUi
{
    public class PlaceTheBoats
    {
        public string BoatsLocation(BattleShip game, string playerName)
        {
            var place = new CanInsertBoat();
            var boats = new BoatCount();
            game.CanInsert = true;

            Console.Clear();
            // if (game.WhoWillPlaceTheShips == "P")
            // {
                System.Console.WriteLine($"{playerName} please insert your ships!");
            // }

            var allBoats = boats.BoatsCount(game).OrderBy(x => x.Width)
                .Reverse();
            foreach (var each in allBoats)
            {
                var x = 0;
                var y = 0;
                var direction = "";


                var board = game.GetBoard(playerName);
                BattleShipConsoleUi.DrawBoard(board);

                if (game.WhoWillPlaceTheShips == "A" || (playerName == "AI"))
                {
                    do
                    {
                        Random random = new Random();
                        x = random.Next(0, game.GetWidth());
                        y = random.Next(0, game.GetHeight());
                        var num = random.Next(1, 2);
                        if (num == 1 )
                        {
                            direction = "R";
                        }
                        else
                        {
                            direction = "D";
                        }

                        place.BoatLocationCheck(game, x, y, each.Width, direction, playerName);

                    }while (x > game.GetWidth() - 1 || y > game.GetHeight() - 1 || !game.CanInsert);

                }

                if (game.WhoWillPlaceTheShips == "M" && playerName != "AI")
                {

                    do
                    {
                        game.CanInsert = true;
                        Console.ForegroundColor = Color.Aqua;
                        System.Console.Write($"Enter {each.Name} size={each.Width} location: ");
                        (x, y) = MoveCoordinates.GetMoveCoordinates(game);

                        if (each.Width > 1 && x < game.GetWidth() && y < game.GetHeight() )
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

                        if (x < game.GetWidth() || y < game.GetHeight())
                        {
                            place.BoatLocationCheck(game, x, y, each.Width, direction, playerName);

                        }

                        if (game.CanInsert) continue;
                        Console.ForegroundColor = Color.Maroon;
                        System.Console.WriteLine("You can't place your ship here!");
                        Console.ForegroundColor = Color.Blue;

                    } while (x > game.GetWidth() - 1 || y > game.GetHeight() - 1 || !game.CanInsert);
                }

                game.InsertBoat(x, y, playerName, each.Width, direction, each.Name);
            }

            return "";


        }
    }
}