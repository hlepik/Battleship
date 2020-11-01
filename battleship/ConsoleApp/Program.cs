using System;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using BattleShipUi;
using DAL;
using Domain;
using GameBrain;
using MenuSystem;
using Console = Colorful.Console;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            // using (var db = new AppDbContext())
            // {
            //     foreach (var board in db.BoardStates!)
            //     {
            //         System.Console.WriteLine(board);
            //     }
            // }

            // for (int j = 0; j < 100; j++)
            // {
            //     Console.Clear();
            //
            //     // steam
            //     Console.Write("       . . . . o o o o o o", Color.LightGray);
            //     for (int s = 0; s < j / 2; s++)
            //     {
            //         Console.Write(" o", Color.LightGray);
            //     }
            //     Console.WriteLine();
            //
            //     var margin = "".PadLeft(j);
            //     Console.WriteLine(margin + "                 ____      o",    Color.DeepSkyBlue);
            //     Console.WriteLine(margin + "                 |  |      o",    Color.DeepSkyBlue);
            //     Console.WriteLine(margin + "      _____====__|OO|_n_n__]___   ", Color.DeepSkyBlue);
            //     Console.WriteLine(margin + "      \\___[]__[]__[]__[]__[]__/ ", Color.DeepSkyBlue);
            //     Console.WriteLine(margin + "      \\______________________/", Color.Blue);
            //     Console.WriteLine("≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈" +
            //                       "≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈",
            //         Color.DarkBlue);
            //
            //     Thread.Sleep(150);
            // }



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

            var input = new UserInput();
            var rules = new Rules();
            var insert = new InsertingBoats();
            var afterHit = new NextMoveAfterHit();

            var player1 = "";
            var player2 = "";
            // (player1, player2) = input.AskName();
            player1 = "helen";
            player2 = "lelen";
            var (width, height) = input.BoardSize();

            var gameRules = rules.GameRules();
            // afterHit.NextMoveAfterHitRule();
            // insert.InsertingBoat();
            Console.Clear();
            var game = new BattleShip(width, height, player1, player2, gameRules);
            Game(game);

            return "";
        }


        static string Game(BattleShip game)
        {
            var boats = new PlaceTheBoats();
            var board1 = game.GetBoard(game.GetPlayer1());
            var board2 = game.GetBoard(game.GetPlayer2());

            boats.BoatsLocation(game, game.GetPlayer1());
            boats.BoatsLocation(game, game.GetPlayer2());


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

            var (x, y) = MoveCoordinates.GetMoveCoordinates(game);
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
            Console.SetCursorPosition( 4, Console.CursorTop);
            System.Console.WriteLine($"{name.ToUpper()}'s board");

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
