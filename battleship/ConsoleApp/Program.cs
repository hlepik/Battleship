using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Channels;
using MenuSystem;
using Console = Colorful.Console;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)


        {

            int red = 200;
            int green = 100;
            int blue = 255;

            Console.WriteAscii("BATTLESHIP", Color.FromArgb(red, green, blue));

            var menu2 = new Menu(MenuLevel.Level2Plus);
            menu2.AddMenuItem(new MenuItem("New game human vs human", "1", BattleShip));
            menu2.AddMenuItem(new MenuItem("New game human vs AI", "2", BattleShip));
            menu2.AddMenuItem(new MenuItem("New game AI vs AI", "3", BattleShip));

            var menu1 = new Menu(MenuLevel.Level1);
            menu1.AddMenuItem(new MenuItem("Play a game", "1", menu2.RunMenu));
            menu1.AddMenuItem(new MenuItem("Return to saved game", "2", LoadGame));

            var menu = new Menu(MenuLevel.Level0);
            menu.AddMenuItem(new MenuItem("Choose a game", "1", menu1.RunMenu));

            menu.RunMenu();

        }

        static string LoadGame()
        {
            System.Console.WriteLine("Not implemented yet!");
            return "";
        }

        static string BattleShip()
        {

            return "";
        }
    }
}
