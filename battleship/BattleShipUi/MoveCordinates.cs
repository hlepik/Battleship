
using System;
using System.Collections.Generic;
using System.Drawing;
using GameBrain;
using Console = Colorful.Console;

namespace BattleShipUi
{
    public class MoveCoordinates
    {
        public static int[] AiLastMove { get; set; } = null!;

        public static (int x, int y)  GetMoveCoordinates(BattleShip game)
        {

            var x = 26;
            var y = 26;
            var input = "";
            var letters = string.Empty;


            if (BattleShip.Ai && !game.NextMoveByX)
            {
                Random random = new Random();
                // if (BattleShip.AiHit)
                // {
                //     while (BattleShip.AiHit)
                //     {
                //         x = AiLastMove[0] + 1;
                //         y = AiLastMove[1];
                //     }
                //
                // }
                // else
                {
                    x = random.Next(0, game.GetWidth());
                    y = random.Next(0, game.GetHeight());
                    AiLastMove = new int[] {x, y};
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

            if (x > game.GetWidth() - 1 || y > game.GetHeight() - 1)
            {
                Console.ForegroundColor = Color.Maroon;
                System.Console.WriteLine("Input was not in a correct format!");
                Console.ForegroundColor = Color.Blue;
            }


            return (x, y);
        }
    }
}