using System;
using System.Drawing;
using Domain.Enums;
using Console = Colorful.Console;
namespace BattleShipUi
{
    public class Rules
    {
        public EBoatsCanTouch GameRules()
        {
            var output = EBoatsCanTouch.No;
            var input = "";
            do
            {
                Console.ForegroundColor = Color.Teal;
                Console.WriteLine("Please select the game rules:" + Environment.NewLine +
                                  "A - Ships can't be touching" + Environment.NewLine +
                                  "B - Ships corners can be touching" + Environment.NewLine +
                                  "C - Ship can be touching");

                Console.ForegroundColor = Color.Aqua;
                input = Console.ReadLine().Trim().ToUpper();
                Console.Clear();
                if (input != "A" && input != "B" && input != "C")
                {
                    Console.ForegroundColor = Color.Maroon;
                    Console.WriteLine("Invalid input!");
                    Console.ForegroundColor = Color.Blue;
                }


            } while (input != "A" && input != "B" && input != "C");

            output = input switch
            {
                "B" => EBoatsCanTouch.Corner,
                "C" => EBoatsCanTouch.Yes,
                _ => output
            };

            return output;
        }

    }
}