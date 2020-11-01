using System;
using System.Drawing;
using Domain.Enums;
using Console = Colorful.Console;
namespace BattleShipUi
{
    public class Rules
    {
        public string GameRules()
        {
            var input = "";
            do
            {
                Console.Clear();
                Console.ForegroundColor = Color.Teal;
                Console.WriteLine("Please select the game rules:" + Environment.NewLine +
                                  "A - Ships can't be touching" + Environment.NewLine +
                                  "B - Ships corners can be touching" + Environment.NewLine +
                                  "C - Ship can be touching");

                Console.ForegroundColor = Color.Aqua;
                input = Console.ReadLine().Trim().ToUpper();
                if (input != "A" && input != "B" && input != "C")
                {
                    Console.ForegroundColor = Color.Maroon;
                    Console.WriteLine("Invalid input!");
                    Console.ForegroundColor = Color.Blue;
                }

            } while (input != "A" && input != "B" && input != "C");

            input = input switch
            {
                "A" => EBoatsCanTouch.No.ToString(),
                "B" => EBoatsCanTouch.Corner.ToString(),
                "C" => EBoatsCanTouch.Yes.ToString()
            };
            return input;
        }

    }
}