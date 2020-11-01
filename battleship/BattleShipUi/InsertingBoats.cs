using System;
using System.Drawing;
using Console = Colorful.Console;

namespace BattleShipUi
{
    public class InsertingBoats
    {
        public string InsertingBoat()
        {

            Console.ForegroundColor = Color.ForestGreen;
            Console.WriteLine("Do you want to insert your own ships or automatically?" +Environment.NewLine +
                              "P - Place your own ships"+ Environment.NewLine +
                              "A - Automatically");

            var input = Console.ReadLine().Trim().ToUpper();


            return input;

        }
    }
}