using System;
using System.Drawing;
using System.Threading;
using BattleShipUi;
using DAL;
using Domain;
using GameBrain;
using MenuSystem;
using Microsoft.EntityFrameworkCore;
using Console = Colorful.Console;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new AppDbContext();
            db.Database.Migrate();


            for (int j = 0; j < 100; j++)
            {
                Console.Clear();

                // steam
                Console.Write("       . . . . o o o o o o", Color.LightGray);
                for (int s = 0; s < j / 2; s++)
                {
                    Console.Write(" o", Color.LightGray);
                }
                Console.WriteLine();

                var margin = "".PadLeft(j);
                Console.WriteLine(margin + "                 ____      o",    Color.DeepSkyBlue);
                Console.WriteLine(margin + "                 |  |      o",    Color.DeepSkyBlue);
                Console.WriteLine(margin + "      _____====__|OO|_n_n__]___   ", Color.DeepSkyBlue);
                Console.WriteLine(margin + "      \\___[]__[]__[]__[]__[]__/ ", Color.DeepSkyBlue);
                Console.WriteLine(margin + "      \\______________________/", Color.Blue);
                Console.WriteLine("≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈" +
                                  "≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈",
                    Color.DarkBlue);

                Thread.Sleep(100);
            }



            int red = 200;
            int green = 100;
            int blue = 255;


            Console.WriteAscii("BATTLESHIP", Color.FromArgb(red, green, blue));


            var menu2 = new Menu(2);
            menu2.AddMenuItem(new MenuItem("New game human vs human", "1", Parameters));
            menu2.AddMenuItem(new MenuItem("New game human vs AI", "2", Ai));
            menu2.AddMenuItem(new MenuItem($"Load game", userChoice: "L", () => { return LoadGameAction(); })
            );

            var menu1 = new Menu(1);
            menu1.AddMenuItem(new MenuItem("BattleShip", "1", menu2.RunMenu!));

            var menu = new Menu();
            menu.AddMenuItem(new MenuItem("Choose game", "1", menu1.RunMenu!));

            menu.RunMenu();

        }

        static string Ai()
        {
            BattleShip.Ai = true;
            Parameters();
            return "";
        }

        static string Parameters()
        {
            Console.Clear();
            var boats = new PlaceTheBoats();
            var input = new UserInput();
            var rules = new Rules();
            var nextMove = new NextMoveAfterHit();

            var (player1, player2) = input.AskName();

            var (width, height) = input.BoardSize();
            var gameRules = rules.GameRules();
            var game = new BattleShip(width, height, player1, player2, gameRules);
            game.NextMove = nextMove.NextMoveAfterHitRule();
            Console.Clear();
            boats.BoatsLocation(game, player1);
            boats.BoatsLocation(game, player2);
            if (BattleShip.Ai)
            {
                game.PlayerType1 = EPlayerType.Human;
                game.PlayerType2 = EPlayerType.Ai;

            }else
            {
                game.PlayerType1 = EPlayerType.Human;
                game.PlayerType2 = EPlayerType.Human;
            }
            PlayGame(game);

            return "";
        }

        private static string PlayGame(BattleShip game)

        {
            Console.Clear();
            var menu = new Menu(3);
            menu.AddMenuItem(new MenuItem(
            $"Next move", userChoice: "N", () =>
            {
                Console.Clear();
                System.Console.WriteLine($"{(game.NextMoveByX ? game.Player1 : game.Player2)}'s " +
                                         "turn! Press enter to continue... ");
                Console.ReadLine();

                var board1 = game.GetBoard(game.Player1);
                var board2 = game.GetBoard(game.Player2);


                Console.Clear();
                var board = board1;

                if (game.NextMoveByX)
                {
                    board = board2;

                }

                if (game.NextMoveByX || game.Player2 != "AI")
                {
                    UserInput.GetTableName(game.Player1);
                    if (game.NextMoveByX)
                    {
                        BattleShipConsoleUi.Hidden = true;
                        BattleShipConsoleUi.DrawBoard(board1);
                        BattleShipConsoleUi.Hidden = false;
                    }
                    else
                    {
                        BattleShipConsoleUi.Hidden = false;
                        BattleShipConsoleUi.DrawBoard(board1);
                        BattleShipConsoleUi.Hidden = true;
                    }

                    UserInput.GetTableName(game.Player2);
                    BattleShipConsoleUi.DrawBoard(board2);
                }

                var bombText = "";
                while (bombText.Length < 1)
                {
                    var y = 0;
                    var x = 0;
                    var boardCell = game.NextMoveByX ? game.Board2 : game.Board1;
                    do
                    {
                        if (game.PlayerType2 != EPlayerType.Ai || game.NextMoveByX)
                        {
                            Console.Write(
                                $"Give Y (A-{Convert.ToChar(game.Height + 64)}) X (1-{game.Width}): ");
                        }

                        (x, y) = MoveCoordinates.GetMoveCoordinates(game);

                    } while (x > game.Width - 1 || y > game.Height - 1 || boardCell[x, y].Miss || boardCell[x, y].Bomb);

                    game.MakeAMove(x, y, board, game);

                    if (game.TextWhenMiss)
                    {
                        bombText = "You missed!";
                    }else
                    {
                        bombText = game.TextWhenHit ? "Hit" : "Ship has been destroyed!";
                    }


                }
                Console.Clear();
                Console.ForegroundColor = Color.Purple;
                System.Console.WriteLine(bombText);
                Console.WriteLine();

                if (BattleShip.GameIsOver)
                {

                    int red = 200;
                    int green = 100;
                    int blue = 255;

                    Console.WriteAscii($"{(game.NextMoveByX ? game.Player1 : game.Player2)} WON!",
                        Color.FromArgb(red, green, blue));


                }

                return "";
            }));

            Console.ForegroundColor = Color.Blue;

            menu.AddMenuItem(new MenuItem($"Save game", userChoice: "S",
                () => { return SaveGameAction(game); } ));

            menu.RunMenu();


            return "";

    }

        static string LoadGameAction()
        {

            using var db = new AppDbContext();
            int count = 1;


            foreach (var mm in db.Games!.Include(p =>p.GameOption))
            {
                Console.WriteLine($"{count } - {mm.GameOption.Name} {mm.Date} ");

                count++;
            }

            var fileNumber = Console.ReadLine();
            count = 1;
            var gameId = 0;
            foreach (var games in db.Games!){

                if (count == int.Parse(fileNumber))
                {
                    gameId = games.GameId;
                }

                count++;
            }
            PlayGame(GetGame.GetGameFromDb(gameId));
            return "";
        }

        static string SaveGameAction(BattleShip game)
        {
            if (game.GameId != 0)
            {
                var update = new GameUpdate();
                update.Update(game);
                return "";
            }
            var defaultName = "save_" + DateTime.Now.ToString("yyyy-MM-dd") + ".json";
            Console.Write($"File name ({defaultName}):");
            game.FileName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(game.FileName))
            {
                game.FileName = defaultName;
            }

            var serializedGame = game.SaveGameToDb();

            Console.WriteLine(serializedGame);
            System.IO.File.WriteAllText(game.FileName, serializedGame);

            return "";
        }
    }
}
