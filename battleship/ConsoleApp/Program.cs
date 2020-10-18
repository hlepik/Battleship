using System;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using BattleShipUi;
using GameBrain;
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

            var menu2 = new Menu(2);
            menu2.AddMenuItem(new MenuItem("New game human vs human", "1", Parameters));
            menu2.AddMenuItem(new MenuItem("New game human vs AI", "2", Parameters));
            menu2.AddMenuItem(new MenuItem("New game AI vs AI", "3", Parameters));
            menu2.AddMenuItem(new MenuItem($"Load game", userChoice: "L", () => { return LoadGameAction(); })
            );

            var menu1 = new Menu(1);
            menu1.AddMenuItem(new MenuItem("Play game", "1", menu2.RunMenu));

            var menu = new Menu();
            menu.AddMenuItem(new MenuItem("Choose game", "1", menu1.RunMenu));

            menu.RunMenu();


        }

        static string Parameters()
        {
            var player1 = "";
            var player2 = "";
            var width = 0;
            var height = 0;
            (player1, player2) = AskName();
            (width, height) = BoardSize();
            var game = new BattleShip(width, height, player1, player2);
            Game(game);

            return "";
        }


        static string Game(BattleShip game)
        {

            var board1 = game.GetBoard(game.GetPlayer1());
            var board2 = game.GetBoard(game.GetPlayer2());

            var userChoice = "";

            Console.Clear();

            GetTableName(game.GetPlayer1());
            BattleShipConsoleUi.DrawBoard(board1);
            GetTableName(game.GetPlayer2());
            BattleShipConsoleUi.DrawBoard(board2);
            Console.ForegroundColor = Color.Purple;
            System.Console.WriteLine($"{(game.NextMoveByX ? game.GetPlayer1().ToUpper() : game.GetPlayer2().ToUpper())}'s turn!");
            Console.ForegroundColor = Color.Blue;

            var board = board2;

            if (game.NextMoveByX)
            {
                board = board1;
            }

            var (x, y) = GetMoveCordinates(game);
            Console.WriteLine();
            game.MakeAMove(x, y, board);

            Console.ForegroundColor = Color.Purple;

            GetTableName(game.GetPlayer1());
            BattleShipConsoleUi.DrawBoard(board1);
            GetTableName(game.GetPlayer2());
            BattleShipConsoleUi.DrawBoard(board2);

            var menu = new Menu(3);
            menu.AddMenuItem(new MenuItem($"Save game", userChoice: "S",
                () => { return SaveGameAction(game); }));
            menu.AddMenuItem(new MenuItem($"Next move", userChoice: "N",
                () => { return Game(game); }));

            menu.RunMenu();
            Console.Clear();


            return userChoice;
        }

        public static void GetTableName(string name)
        {
            var game = new BattleShip();
            Console.ForegroundColor = Color.Purple;
            Console.SetCursorPosition( 5 * 2, Console.CursorTop);
            System.Console.WriteLine($"{name.ToUpper()}'s board");

        }

        static Tuple<string, string>  AskName()
        {

            System.Console.WriteLine("Please enter Player 1 name!");
            var player1= Console.ReadLine();

            while (player1 .Length < 2 || player1.Length > 100)
            {

                Console.ForegroundColor = Color.Maroon;
                System.Console.WriteLine($"Player name {player1} is too short or too long! " +
                                         $"Name length has to be between 2 to 100!");
                Console.ForegroundColor = Color.Blue;
                System.Console.WriteLine("Please enter Player 1 name!");
                player1  = Console.ReadLine();
            }

            System.Console.WriteLine("Please enter Player 2 name!");
            var player2 = Console.ReadLine();

            while (player2.Length < 2 || player2.Length > 100)
            {

                Console.ForegroundColor = Color.Maroon;
                System.Console.WriteLine($"Player name {player2} is too short or too long! " +
                                         $"Name length has to be between 2 to 100!");
                Console.ForegroundColor = Color.Blue;
                System.Console.WriteLine("Please enter Player 2 name!");
                player2 = Console.ReadLine();
            }
            while (player1 == player2)
            {
                Console.ForegroundColor = Color.Maroon;
                System.Console.WriteLine("Players name can't be same!");
                Console.ForegroundColor = Color.Blue;
                System.Console.WriteLine("Please enter Player 2 name!");
                player2 = Console.ReadLine();
            }

            return new Tuple<string, string>(player1, player2);
        }

        static Tuple<int, int>  BoardSize()
        {
            var width = 0;
            var height = 0;
            while (width < 5 || width > 25)
            {
                Console.WriteLine("Please insert game board width between 5 to 25!");
                var userChoice = Console.ReadLine();
                if (int.TryParse(userChoice, out _))
                {
                    width = Int32.Parse(userChoice);
                    if (width < 5 || width > 25)
                    {
                        Console.ForegroundColor = Color.Maroon;
                        System.Console.WriteLine("Width not between allowed range!");
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
                Console.WriteLine("Please insert game board height between 5 to 25!");
                var userChoice = Console.ReadLine();
                if (int.TryParse(userChoice, out _))
                {
                    height = Int32.Parse(userChoice);
                    if (height < 5 ||height > 25)
                    {
                        Console.ForegroundColor = Color.Maroon;
                        System.Console.WriteLine("Height not between allowed range!");
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

        static (int x, int y) GetMoveCordinates(BattleShip game)
        {

            var x = 26;
            var y = 26;
            var input = "";

            while(x > game.GetWidth() -1 || y > game.GetHeight()-1 || input.Length < 2 || input.Length > 3){
                Console.WriteLine("Upper left corner is (A 1)!");
                Console.Write($"Give Y A-{Convert.ToChar(game.GetWidth() + 64)}, X 1-{game.GetHeight()}:");
                var letters = string.Empty;

                input = Console.ReadLine().Trim();

                if (!String.IsNullOrWhiteSpace(input) && Char.IsLetter(input[0]) && Char.IsDigit(input[1]))
                {


                    foreach (var t in input)
                    {
                        if (Char.IsNumber(t))
                        {
                            letters += t.ToString();
                        }
                        else if (char.IsLetter(t))
                        {
                            y = char.ToUpper(input[0]) - 65;
                        }
                    }

                    x = int.Parse(letters) - 1;
                }

                if (input.Length > 3 || input.Length < 2 || x > game.GetWidth() -1 || y > game.GetHeight()-1)
                {
                    Console.ForegroundColor = Color.Maroon;
                    System.Console.WriteLine("Input string was not in a correct format!");
                    Console.ForegroundColor = Color.Blue;

                }
            }

            return (x, y);
        }

        static string LoadGameAction()
        {
            var game = new BattleShip();
            var files = System.IO.Directory.EnumerateFiles(".", "*.json").ToList();
            for (int i = 0; i < files.Count; i++)
            {
                Console.WriteLine($"{i} - {files[i]}");
            }

            var fileNo = Console.ReadLine();
            var fileName = files[int.Parse(fileNo!.Trim())];

            var jsonString = System.IO.File.ReadAllText(fileName);

            game.SetGameStateFromJsonString(jsonString);

            Game(game);
            return "";
        }

        static string SaveGameAction(BattleShip game)
        {
            // 2020-10-12
            var defaultName = "save_" + DateTime.Now.ToString("yyyy-MM-dd") + ".json";
            Console.Write($"File name ({defaultName}):");
            var fileName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = defaultName;
            }

            var serializedGame = game.GetSerializedGameState();

            Console.WriteLine(serializedGame);
            System.IO.File.WriteAllText(fileName, serializedGame);

            return "";
        }


    }


}
