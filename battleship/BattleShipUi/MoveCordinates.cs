
using System;
using System.Drawing;
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

            while(x > game.GetWidth() -1 || y > game.GetHeight()-1 || input.Length < 2 || input.Length > 3){
                // Console.WriteLine("Upper left corner is (A 1)!");
                // Console.Write($"Give Y A-{Convert.ToChar(game.GetWidth() + 64)}, X 1-{game.GetHeight()}:");
                var letters = string.Empty;
                Console.ForegroundColor = Color.Aqua;
                input = Console.ReadLine().Trim();
                Console.ForegroundColor = Color.Blue;

                if (!String.IsNullOrWhiteSpace(input) && input.Length > 1 && input.Length <= 3 && Char.IsLetter(input[0]) && Char.IsDigit(input[1]))
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

                if (x > game.GetWidth() -1 || y > game.GetHeight()-1)
                {
                    Console.ForegroundColor = Color.Maroon;
                    System.Console.WriteLine("Input was not in a correct format!");
                    Console.ForegroundColor = Color.Blue;
                }
            }

            return (x, y);

        }

    }

}