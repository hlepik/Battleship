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

            Console.Clear();
            System.Console.WriteLine($"{playerName} please insert your ships!");
            var allBoats = boats.BoatsCount(game).OrderBy(x => x.Width)
                .Reverse();
            foreach (var each in allBoats)
            {
                // var input = "";
                var x = 26;
                var y = 26;
                var direction = "";

                if (playerName == game.GetPlayer1())
                {
                    var board1 = game.GetBoard(game.GetPlayer1());
                    BattleShipConsoleUi.DrawBoard(board1);
                }
                else
                {
                    var board2 = game.GetBoard(game.GetPlayer2());
                    BattleShipConsoleUi.DrawBoard(board2);
                }


                // do
                // {

                do
                {
                    game.CanInsert = true;
                    Console.ForegroundColor = Color.Aqua;
                    System.Console.Write($"Enter {each.Name} size={each.Width} location: ");
                    // input = Console.ReadLine().Trim();
                    // var letters = string.Empty;
                    (x, y) = MoveCoordinates.GetMoveCoordinates(game);

                    // if (!String.IsNullOrWhiteSpace(input) && input.Length > 1 && input.Length <= 3 && Char.IsLetter(input[0]) && Char.IsDigit(input[1]))
                    // {
                    //     foreach (var t in input)
                    //     {
                    //         if (Char.IsNumber(t))
                    //         {
                    //             letters += t.ToString();
                    //         }
                    //         else if (char.IsLetter(t))
                    //         {
                    //             y = char.ToUpper(input[0]) - 65;
                    //         }
                    //     }
                    //
                    //     x = int.Parse(letters) - 1;
                    // }
                    // else if (input.Length > 3 || input.Length < 2 || x > game.GetWidth() - 1 || y > game.GetHeight() - 1)
                    // {
                    //     Console.ForegroundColor = Color.Maroon;
                    //     System.Console.WriteLine("Input was not in a correct format!");
                    //     Console.ForegroundColor = Color.Blue;
                    //
                    // }

                    // } while (x > game.GetWidth() && y > game.GetHeight());

                    if (each.Width > 1)
                    {

                        do
                        {
                            System.Console.Write($"Enter {each.Name} direction R = right, D = down: ");
                            direction = Console.ReadLine().Trim().ToUpper();
                            if (direction != "R" && direction != "D")
                            {
                                Console.ForegroundColor = Color.Maroon;
                                System.Console.WriteLine("Invalid input!");
                                Console.ForegroundColor = Color.Blue;

                            }

                        } while (direction != "R" && direction != "D");

                    }

                    if (x <= game.GetWidth() - 1 || y <= game.GetHeight() - 1)
                    {
                        place.GameAPlacement(game, x, y, each.Width, direction, playerName);

                    }

                    if (!game.CanInsert)
                    {
                        Console.ForegroundColor = Color.Maroon;
                        System.Console.WriteLine("You can't place your ship here!");
                        Console.ForegroundColor = Color.Blue;
                    }

                } while (x > game.GetWidth() - 1 || y > game.GetHeight() - 1|| !game.CanInsert);


                game.InsertBoat(x, y, playerName, each.Width, direction);

            }

            return "";


        }
    }
}