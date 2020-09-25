using System;
using System.Collections.Generic;
using MenuSystem;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;

            var menu2 = new Menu(2)
            {
                SubTitle = "BattleShip",
                MenuItems = new List<MenuItem>()
                {
                    new MenuItem()
                    {
                        UserChoice = "1",
                        Title = "Play game: Human vs Human",
                        MethodToExecute = DefaultMenuAction
                    },
                    new MenuItem()
                    {
                        UserChoice = "2",
                        Title = "Play game: Human vs AI",
                        MethodToExecute = DefaultMenuAction
                    },
                    new MenuItem()
                    {
                        UserChoice = "3",
                        Title = "Play game: AI vs AI",
                        MethodToExecute = DefaultMenuAction
                    },
                }

            };
            var menu1 = new Menu(1)
            {
                SubTitle = "Game menu",
                MenuItems = new List<MenuItem>()
                {
                    new MenuItem()
                    {
                        UserChoice = "1",
                        Title = "BattleShip",
                        MethodToExecute = menu2.Run,
                    },
                }
            };


            var menu0 = new Menu()

            {

                SubTitle = "Main menu",
                MenuItems = new List<MenuItem>()
                {
                    new MenuItem()
                    {
                        UserChoice = "1",
                        Title = "Play a game",
                        MethodToExecute = menu1.Run
                    },

                }
            };
            menu0.Run();

        }


        static void DefaultMenuAction()
        {
            Console.WriteLine("Not implemented yet!");
        }
    }
}



