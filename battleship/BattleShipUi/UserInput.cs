using System;
using System.Drawing;
using GameBrain;
using Console = Colorful.Console;

namespace BattleShipUi
{

    public class UserInput
    {

        public Tuple<string, string>  AskName()
        {

            var player1 = "";
            var player2 = "AI";

            while (player1.Length < 2 || player1.Length > 128)
            {
                Console.ForegroundColor = Color.Blue;
                var name = "Please enter Player 1 name: ";
                for (int i = 0; i < name.Length; i++)
                {
                    Console.Write(name[i]);

                    System.Threading.Thread.Sleep(50);
                }

                Console.ForegroundColor = Color.Aqua;
                player1 = Console.ReadLine().ToUpper();
                Console.ForegroundColor = Color.Blue;

                if (player1.Length < 2 || player1.Length > 128)
                {
                    Console.ForegroundColor = Color.Maroon;
                    System.Console.WriteLine($"Player name {player1} is too short or too long! " +
                                             $"Name length has to be between 2 to 100!");
                    Console.ForegroundColor = Color.Blue;
                }
                if (BattleShip.Ai) return new Tuple<string, string>(player1, player2);
                player2 = "";


            while (player2.Length < 2  || player2.Length > 128)
            {
                Console.ForegroundColor = Color.Blue;
                name = "Please enter Player 2 name: ";
                for (int i = 0; i < name.Length; i++)
                {
                    Console.Write(name[i]);

                    System.Threading.Thread.Sleep(50);
                }

                Console.ForegroundColor = Color.Aqua;
                player2 = Console.ReadLine().ToUpper();
                Console.ForegroundColor = Color.Blue;

                if (player2.Length < 2 || player2.Length > 128)
                {
                    Console.ForegroundColor = Color.Maroon;
                    System.Console.WriteLine($"Player name {player2} is too short or too long! " +
                                             $"Name length has to be between 2 to 100!");
                    Console.ForegroundColor = Color.Blue;
                }
            }

            if (player1 == player2)
            {
                do
                {
                    Console.ForegroundColor = Color.Maroon;
                    name = "Players name can't be same! Please insert Player 2 name: ";
                    Console.ForegroundColor = Color.Blue;
                    for (int i = 0; i < name.Length; i++)
                    {
                        Console.Write(name[i]);

                        System.Threading.Thread.Sleep(50);
                    }

                    Console.ForegroundColor = Color.Aqua;
                    player2 = Console.ReadLine().ToUpper();
                    Console.ForegroundColor = Color.Blue;
                } while (player2.Length > 2 || player2.Length < 128 || player1 != player2);
            }
            }

            return new Tuple<string, string>(player1, player2);
        }

        public Tuple<int, int>  BoardSize()
        {
            var width = 0;
            var height = 0;
            while (width < 5 || width > 25)
            {
                var widthInput = "Please insert board width. It must be a number between 5 and 25: ";
                for (int i = 0; i < widthInput.Length; i++) {
                    Console.Write(widthInput[i]);

                    System.Threading.Thread.Sleep(40);
                }
                Console.ForegroundColor = Color.Aqua;
                var userChoice = Console.ReadLine();
                Console.ForegroundColor = Color.Blue;
                if (int.TryParse(userChoice, out _))
                {
                    width = Int32.Parse(userChoice);
                    if (width < 5 || width > 25)
                    {
                        Console.ForegroundColor = Color.Maroon;
                        System.Console.WriteLine("Width is not within the permitted range!");
                        Console.ForegroundColor = Color.Blue;
                    }
                }
                else
                {
                    Console.ForegroundColor = Color.Maroon;
                    Console.WriteLine("Incorrect width. Width has to be a number!");
                    Console.ForegroundColor = Color.Blue;
                }

            }

            while (height < 5 || height > 25)
            {

                var heightInput = "Please insert board height. It must be a number between 5 and 25: ";
                for (int i = 0; i < heightInput.Length; i++)
                {
                    Console.Write(heightInput[i]);
                    System.Threading.Thread.Sleep(40);
                }

                Console.ForegroundColor = Color.Aqua;
                var userChoice = Console.ReadLine();
                Console.ForegroundColor = Color.Blue;
                if (int.TryParse(userChoice, out _))
                {
                    height = Int32.Parse(userChoice);
                    if (height < 5 ||height > 25)
                    {
                        Console.ForegroundColor = Color.Maroon;
                        System.Console.WriteLine("Height is not within the permitted range!");
                        Console.ForegroundColor = Color.Blue;
                    }
                }
                else
                {
                    Console.ForegroundColor = Color.Maroon;
                    Console.WriteLine("Incorrect height. Height has to be a number!");
                    Console.ForegroundColor = Color.Blue;
                }
            }

            return new Tuple<int, int>(width, height);

        }

        public static void GetTableName(string name)
        {
            Console.ForegroundColor = Color.Purple;
            Console.SetCursorPosition( 4, Console.CursorTop);
            System.Console.WriteLine($"{name.ToUpper()}'s board");

        }

    }
}