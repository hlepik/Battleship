
using System;
using System.Drawing;
using Domain;
using GameBrain;
using Console = Colorful.Console;

namespace BattleShipUi
{
    public class MoveCoordinates
    {

        public static (int x, int y)  GetMoveCoordinates(BattleShip game)
        {

            var x = 26;
            var y = 26;
            var input = "";
            var letters = string.Empty;


            if (game.PlayerType2 == EPlayerType.Ai && !game.NextMoveByX)
            {
                Random random = new Random();

                {
                    x = random.Next(0, game.Width);
                    y = random.Next(0, game.Height);

                }
            }
            else
            {
                Console.ForegroundColor = Color.Aqua;
                input = Console.ReadLine().Trim();
                Console.ForegroundColor = Color.Blue;
            }


            if (!String.IsNullOrWhiteSpace(input) && input.Length > 1 && input.Length <= 3 &&
                Char.IsLetter(input[0]) && Char.IsDigit(input[1]))
            {

                foreach (var t in input)
                {
                    if (Char.IsNumber(t))
                    {
                        letters += t.ToString();
                    }
                    else if (char.IsLetter(t))
                    {
                        y = char.ToUpper(input[0]) - 65;
                    }
                }

                x = int.Parse(letters) - 1;
            }

            if (x > game.Width- 1 || y > game.Height - 1)
            {
                Console.ForegroundColor = Color.Maroon;
                System.Console.WriteLine("Input was not in a correct format!");
                Console.ForegroundColor = Color.Blue;
            }

            var board = game.NextMoveByX ? game.Board2 : game.Board1;
            if (x <= game.Width- 1 &&  y <= game.Height - 1)
            {
                if (board[x, y].Miss || board[x, y].Bomb)
                {
                    Console.ForegroundColor = Color.Maroon;
                    System.Console.WriteLine("Already marked!");
                    Console.ForegroundColor = Color.Blue;
                }

            }

            return (x, y);
        }
    }
}