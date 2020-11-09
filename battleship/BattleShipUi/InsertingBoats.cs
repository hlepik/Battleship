using System;
using System.Drawing;
using Console = Colorful.Console;

namespace BattleShipUi
{
    public class InsertingBoats

    {
        private bool _canInsert;
        // public bool CanInsert
        // {
        //     get => _canInsert;
        //     set => _canInsert = value;
        // }
        public string InsertingBoat()
        {
            var input = "";

            do
            {
                Console.ForegroundColor = Color.ForestGreen;
                Console.WriteLine("Do you want to insert your own ships or automatically?" + Environment.NewLine +
                                  "A - Automatically" + Environment.NewLine +
                                  "M - Place your own ships");

                input = Console.ReadLine().Trim().ToUpper();

                Console.Clear();
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