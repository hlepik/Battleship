using System;
using System.Collections.Generic;
using Console = Colorful.Console;
using System.Drawing;
using System.Linq;
using BattleShipUi;
using GameBrain;


namespace MenuSystem
{


    public class Menu
    {

        private Dictionary<string, MenuItem> MenuItems { get; set; } = new Dictionary<string, MenuItem>();


        // private readonly MenuLevel _menuLevel;
        private int _menuLevel;

        private MenuItem? _menuItemReturnPrevious;
        private MenuItem? _menuItemReturnMain;
        private MenuItem _menuItemExit = new MenuItem("Exit", "X", () => "");

        public Menu(int menuLevel = 0)
        {

            _menuLevel = menuLevel;


            if (_menuLevel >= 1 && _menuLevel < 3)
            {
                _menuItemReturnMain = new MenuItem("Return to main", "M", () => "");
            }

            if (_menuLevel >= 2 && _menuLevel < 3)
            {
                _menuItemReturnPrevious = new MenuItem("Return to previous", "R", () => "");
            }

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

        public object? Title { get; set; }

        public string RunMenu()
        {

            var userChoice = "";

            do

            {

                Console.ForegroundColor = Color.DarkBlue;
                if (_menuLevel == 0)
                {
                    Title = "Main Menu";
                }
                else if (_menuLevel == 1)
                {
                    Title = "Level 1 Menu";
                }
                else if(_menuLevel == 2)
                {
                    Title = "BattleShip";

                }

                Console.ForegroundColor = Color.DarkBlue;

                if (_menuLevel < 3)
                {
                    Console.WriteLine($"====== {Title} ======");
                }
                Console.ForegroundColor = Color.Blue;

                if (!BattleShip.GameIsOver)
                {
                    foreach (var menuItem in MenuItems.Values)
                    {
                        Console.WriteLine(menuItem);
                    }
                }
                else
                {
                    BattleShip.GameIsOver = false;
                }

                Console.WriteLine("------------------------");
                if (_menuItemReturnPrevious != null)
                {
                    Console.ForegroundColor = Color.Cyan;
                    Console.WriteLine(_menuItemReturnPrevious);
                }

                if (_menuItemReturnMain != null)
                {
                    Console.ForegroundColor = Color.Green;
                    Console.WriteLine(_menuItemReturnMain);
                }


                Console.ForegroundColor = Color.DarkGreen;
                Console.WriteLine(_menuItemExit);
                Console.ForegroundColor = Color.Blue;
                Console.WriteLine("------------------------");

                Console.ForegroundColor = Color.Aqua;
                Console.Write(">");
                userChoice = Console.ReadLine()?.Trim().ToUpper() ?? "";

                if (_menuItemExit.UserChoice != userChoice && _menuItemReturnMain?.UserChoice != userChoice
                 && _menuItemReturnPrevious?.UserChoice != userChoice)

                {
                    if (MenuItems.TryGetValue(userChoice, out var userMenuItem))
                    {
                        userChoice = userMenuItem?.MethodToExecute!();
                    }
                    else
                    {
                        Console.WriteLine("I don't have this option!");
                    }
                }
                if (userChoice == _menuItemExit.UserChoice)
                {
                    if (_menuLevel == 0)
                    {
                        int red = 200;
                        int green = 100;
                        int blue = 255;

                        Console.WriteAscii("CLOSING DOWN !!!", Color.FromArgb(red, green, blue));
                    }

                    break;
                }

                if (_menuLevel >= 2 && userChoice == _menuItemReturnPrevious?.UserChoice)
                {
                    userChoice = string.Empty;
                    break;
                }

                if (_menuLevel != 0 && userChoice == _menuItemReturnMain?.UserChoice)
                {
                    break;
                }


            } while (userChoice != _menuItemExit.UserChoice);

            return userChoice;

        }

    }
}
