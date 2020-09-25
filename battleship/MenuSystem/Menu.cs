using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuSystem
{
    public class Menu
    {
        public string SubTitle { get; set; } = null!;
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        private readonly MenuItem _menuItemExit = new MenuItem()

        {
            UserChoice = "X",
            Title = "Exit"

        };

        private readonly MenuItem _returnPrevious = null!;
        private readonly MenuItem _returnMain = null!;
        private int _level;

        public Menu(int level = 0)
        {
            _level = level;


            if (_level >= 1)
            {
                _returnMain = new MenuItem()
                {
                    UserChoice = "M",
                    Title = "Return to Main menu"
                };
                if (_level >= 2)
                {
                    _returnPrevious = new MenuItem()
                    {
                        UserChoice = "P",
                        Title = "Return to Previous menu"
                    };
                }
            }
        }
        static class Choice
        {
            public static string? Userchoice;
        }

        public void Run()
        {

            do
            {

                Console.WriteLine($"====== {SubTitle} ======");
                foreach (var menuItem in MenuItems)
                {
                    Console.WriteLine(menuItem);

                }
                Console.WriteLine("------------------------");

                if (_returnPrevious != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(_returnPrevious);
                }

                if (_returnMain != null)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(_returnMain);
                }

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(_menuItemExit);
                Console.ForegroundColor = ConsoleColor.Magenta;

                Console.WriteLine("------------------------");

                Console.Write(">");

                Choice.Userchoice = Console.ReadLine()?.Trim().ToUpper() ?? "";



                if (Choice.Userchoice == _menuItemExit.UserChoice)
                {
                    Console.WriteLine("Closing down......");
                    Environment.Exit(0);
                }
                if (Choice.Userchoice == _returnMain?.UserChoice)
                {

                    _level = 0;
                    break;

                }


                var userMenuItem = MenuItems.FirstOrDefault(t => t.UserChoice == Choice.Userchoice);
                if (userMenuItem != null)
                {
                    userMenuItem.MethodToExecute!();
                }
                else
                {
                    Console.WriteLine("I don't have this option!");
                }

            } while (Choice.Userchoice != _menuItemExit.UserChoice && Choice.Userchoice != _returnMain?.UserChoice
                                                                   && Choice.Userchoice != _returnPrevious?.UserChoice);
        }

    }



}