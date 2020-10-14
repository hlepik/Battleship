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
            menu2.AddMenuItem(new MenuItem("New game human vs human", "1", Game));
            menu2.AddMenuItem(new MenuItem("New game human vs AI", "2", Game));
            menu2.AddMenuItem(new MenuItem("New game AI vs AI", "3", Game));

            var menu1 = new Menu(1);
            menu1.AddMenuItem(new MenuItem("Play game", "1", menu2.RunMenu));

            var menu = new Menu();
            menu.AddMenuItem(new MenuItem("Choose game", "1", menu1.RunMenu));

            menu.RunMenu();


        }


        static string DefaultMenuAction()
        {
            Console.WriteLine("Not implemented yet!");

            return "";
        }


        static string Game()
        {

            var game = new BattleShip();

            AskName();
            BoardSize();
            BattleShip.Board1 = game.GetBoard();
            BattleShip.Board2 = game.GetBoard();

            Console.Clear();

            var userChoice = "";

            var menu = new Menu(3);
            menu.AddMenuItem(new MenuItem(
                $"Next move",
                userChoice: "N",
                () =>
                {
                    Console.Clear();
                    while (true)
                    {

                        System.Console.WriteLine($"{(BattleShip.NextMoveByX ? BattleShip.Player1.ToUpper() : BattleShip.Player2.ToUpper())}'s turn!");
                        System.Console.WriteLine("Press Enter to continue!");
                        ConsoleKeyInfo keyInfo = Console.ReadKey();
                        while(keyInfo.Key != ConsoleKey.Enter)
                            keyInfo = Console.ReadKey();
                        {
                            GetTableName(BattleShip.Player1);
                            BattleShipConsoleUi.DrawBoard(BattleShip.Board1);
                            GetTableName(BattleShip.Player2);
                            BattleShipConsoleUi.DrawBoard(BattleShip.Board2);
                        }
                        var board = BattleShip.Board2;

                        if (BattleShip.NextMoveByX)
                        {
                            board = BattleShip.Board1;
                        }

                        var (x, y) = GetMoveCordinates(game);

                    game.MakeAMove(x, y, board);

                    Console.ForegroundColor = Color.Purple;

                    GetTableName(BattleShip.Player1);
                    BattleShipConsoleUi.DrawBoard(BattleShip.Board1);
                    GetTableName(BattleShip.Player2);
                    BattleShipConsoleUi.DrawBoard(BattleShip.Board2);

                    return "";
                    }
                })
            );
            menu.AddMenuItem(new MenuItem(
                $"Save game",
                userChoice: "S",
                () => { return SaveGameAction(game); })
            );
            menu.AddMenuItem(new MenuItem(
                $"Load game",
                userChoice: "L",
                () => { return LoadGameAction(game); })
            );

            userChoice = menu.RunMenu();
            return userChoice;
    }

        public static void GetTableName(string name)
        {

            Console.ForegroundColor = Color.Purple;
            Console.SetCursorPosition(BattleShip.Width * 2 -1, Console.CursorTop);
            System.Console.WriteLine($"{name.ToUpper()}'s board");

        }

        public static void  AskName()
        {

            System.Console.WriteLine("Please enter Player 1 name!");
            BattleShip.Player1 = Console.ReadLine();

            while (BattleShip.Player1 .Length < 2 || BattleShip.Player1 .Length > 100)
            {

                Console.ForegroundColor = Color.Maroon;
                System.Console.WriteLine($"Player name {BattleShip.Player1} is too short or too long! " +
                                         $"Name length has to be between 2 to 100!");
                Console.ForegroundColor = Color.Blue;
                System.Console.WriteLine("Please enter Player 1 name!");
                BattleShip.Player1  = Console.ReadLine();
            }

            System.Console.WriteLine("Please enter Player 2 name!");
            BattleShip.Player2 = Console.ReadLine();

            while (BattleShip.Player2.Length < 2 || BattleShip.Player2.Length > 100)
            {

                Console.ForegroundColor = Color.Maroon;
                System.Console.WriteLine($"Player name {BattleShip.Player2} is too short or too long! " +
                                         $"Name length has to be between 2 to 100!");
                Console.ForegroundColor = Color.Blue;
                System.Console.WriteLine("Please enter Player 2 name!");
                BattleShip.Player2 = Console.ReadLine();
            }
            while (BattleShip.Player1 == BattleShip.Player2)
            {
                Console.ForegroundColor = Color.Maroon;
                System.Console.WriteLine("Players name can't be same!");
                Console.ForegroundColor = Color.Blue;
                System.Console.WriteLine("Please enter Player 2 name!");
                BattleShip.Player2= Console.ReadLine();
            }
        }

        public static void BoardSize()
        {
            while (BattleShip.Width < 5 || BattleShip.Height > 25)
            {
                Console.WriteLine("Please insert game board width between 5 to 25!");
                var userChoice = Console.ReadLine();
                if (int.TryParse(userChoice, out _))
                {
                    BattleShip.Width = Int32.Parse(userChoice);
                    if (BattleShip.Width < 5 || BattleShip.Width > 25)
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

            while (BattleShip.Height < 5 || BattleShip.Height > 25)
            {
                Console.WriteLine("Please insert game board height between 5 to 25!");
                var userChoice = Console.ReadLine();
                if (int.TryParse(userChoice, out _))
                {
                    BattleShip.Height = Int32.Parse(userChoice);
                    if (BattleShip.Height < 5 || BattleShip.Height > 25)
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

        }

        static (int x, int y) GetMoveCordinates(BattleShip game)
        {

            var x = 26;
            var y = 26;
            var input = "";

            while(x > BattleShip.Width -1 || y > BattleShip.Height-1 || input.Length != 2){
                Console.WriteLine("Upper left corner is (A 1)!");
                Console.Write($"Give Y A-{Convert.ToChar(BattleShip.Height + 64)}, X 1-{BattleShip.Height}:");
                var letters = string.Empty;

                input = Console.ReadLine().Trim();

                if (input.Length == 2)
                {
                    foreach (var t in input) {
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
                if (input.Length != 2 || x > BattleShip.Width -1 || y > BattleShip.Height-1)
                {
                    Console.ForegroundColor = Color.Maroon;
                    System.Console.WriteLine("Input string was not in a correct format!");
                    Console.ForegroundColor = Color.Blue;

                }
            }

            return (x, y);
        }

        static string LoadGameAction(BattleShip game)
        {
            var state = new GameState();
            var files = System.IO.Directory.EnumerateFiles(".", "*.json").ToList();
            for (int i = 0; i < files.Count; i++)
            {
                Console.WriteLine($"{i} - {files[i]}");
            }

            var fileNo = Console.ReadLine();
            var fileName = files[int.Parse(fileNo!.Trim())];

            var jsonString = System.IO.File.ReadAllText(fileName);

            game.SetGameStateFromJsonString(jsonString);

            BattleShipConsoleUi.DrawBoard(game.GetBoard( ));
            BattleShipConsoleUi.DrawBoard(game.GetBoard( ));

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
