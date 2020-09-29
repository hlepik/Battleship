using System;
using System.Collections.Generic;
using Console = Colorful.Console;
using System.Drawing;
using System.Linq;


namespace MenuSystem
{

    public enum MenuLevel
    {
        Level0,
        Level1,
        Level2Plus
    }

    public class Menu
    {
        //private  List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        // privaatseks tuleb teha. Parem ja kiirem valik oleks dictionary

        private Dictionary<string, MenuItem> MenuItems { get; set; } = new Dictionary<string, MenuItem>();


        private readonly MenuLevel _menuLevel;

        private readonly string[] _reservedActions = new[] {"x", "m", "r"};



        public Menu(MenuLevel level)
        {
            _menuLevel = level;

        }

        public void AddMenuItem(MenuItem item)
        {

            if (item.UserChoice == "")
            {
                throw new ArgumentException($"UserChoice cannot be empty");
            }

            MenuItems.Add(item.UserChoice, item);

            //crash when the key is already there
        }

        public string RunMenu()
        {

            var userChoice = "";

            do

            {

                Console.ForegroundColor = Color.DarkBlue;
                if (_menuLevel == MenuLevel.Level0)
                {
                    Console.WriteLine("====== Main Menu ======");
                }else if (_menuLevel == MenuLevel.Level1)
                {
                    Console.WriteLine("====== Level 1 Menu ======");
                }
                else
                {
                    Console.WriteLine("====== BattleShip ======");
                }


                Console.ForegroundColor = Color.Blue;
                foreach (var menuItem in MenuItems.Values)
                {
                    Console.WriteLine(menuItem);
                }

                Console.WriteLine("------------------------");
                if (_menuLevel >= MenuLevel.Level1)
                {

                    Console.ForegroundColor = Color.Cyan;
                    Console.WriteLine("M Return to Main");
                    if (_menuLevel >= MenuLevel.Level2Plus)
                    {

                        Console.ForegroundColor = Color.Green;
                        Console.WriteLine("R Return to previous");
                    }
                }

                Console.ForegroundColor = Color.DarkGreen;
                Console.WriteLine("X Exit");
                Console.ForegroundColor = Color.Blue;


                Console.WriteLine("------------------------");


                Console.Write(">");


                userChoice = Console.ReadLine()?.ToLower() ?? "";


                if (!_reservedActions.Contains(userChoice))
                {
                    if (MenuItems.TryGetValue(userChoice, out var userMenuItem))
                    {
                        userChoice = userMenuItem.MethodToExecute();
                    }
                    else
                    {
                        Console.WriteLine("I don't have this option!");
                    }
                }

                if (userChoice == "x")
                {
                    if (_menuLevel == MenuLevel.Level0)
                    {
                        Console.WriteLine("Closing down......");
                    }

                    break;
                }

                if (_menuLevel == MenuLevel.Level2Plus && userChoice == "r")
                {
                    userChoice = string.Empty;
                    break;
                }

                if (_menuLevel != MenuLevel.Level0 && userChoice == "m")
                {
                    break;
                }


            } while (userChoice != "x");

            return userChoice;

            }


        }

}
