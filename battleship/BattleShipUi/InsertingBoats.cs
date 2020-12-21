using System;
using System.Drawing;
using GameBrain;
using Console = Colorful.Console;

namespace BattleShipUi
{
    public class InsertingBoats

    {
        public string InsertingBoat(string player)
        {
            var input = "";

            do
            {
                Console.ForegroundColor = Color.ForestGreen;
                if (player != "AI")
                {

                    Console.WriteLine($"{player} do you want to insert your own ships or automatically?" +
                                      Environment.NewLine +
                                      "A - Automatically" + Environment.NewLine +
                                      "M - Place your own ships");

                    input = Console.ReadLine().Trim().ToUpper();

                    Console.Clear();
                }
                else
                {
                    input = "A";
                }

                if (input != "M" && input != "A")
                {
                    Console.ForegroundColor = Color.Maroon;
                    Console.WriteLine("Invalid input!");
                    Console.ForegroundColor = Color.Blue;
                }

            } while (input != "M" && input != "A");

            return input;

        }
    }
}