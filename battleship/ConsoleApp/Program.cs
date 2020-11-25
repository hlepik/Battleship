using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Threading;
using BattleShipUi;
using DAL;
using Domain;
using Domain.Enums;
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

            {
                using var db = new AppDbContext();
                db.Database.Migrate();

            }

            Console.WriteAscii("BATTLESHIP", Color.FromArgb(red, green, blue));


            var menu2 = new Menu(2);
            menu2.AddMenuItem(new MenuItem("New game human vs human", "1", Parameters));
            menu2.AddMenuItem(new MenuItem("New game human vs AI", "2", Ai));
            menu2.AddMenuItem(new MenuItem($"Load game", userChoice: "L", () => { return LoadGameAction(); })
            );

            var menu1 = new Menu(1);
            menu1.AddMenuItem(new MenuItem("BattleShip", "1", menu2.RunMenu));

            var menu = new Menu();
            menu.AddMenuItem(new MenuItem("Choose game", "1", menu1.RunMenu));

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
            var insert = new InsertingBoats();
            var nextMove = new NextMoveAfterHit();

            var (player1, player2) = input.AskName();

            var (width, height) = input.BoardSize();
            var gameRules = rules.GameRules();
            var game = new BattleShip(width, height, player1, player2, gameRules);
            game.NextMove = nextMove.NextMoveAfterHitRule();
            game.WhoWillPlaceTheShips = insert.InsertingBoat();
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

        static string PlayGame(BattleShip game)

        {
            Console.Clear();
            var menu = new Menu(3);

            menu.AddMenuItem(new MenuItem(
            $"Next move", userChoice: "N", () =>
            {
                Console.Clear();
                System.Console.WriteLine($"" +
                                         $"{(game.NextMoveByX ? game.Player1 : game.Player2)}'s " +
                                         $"turn! Press enter to continue... ");
                Console.ReadLine();

                var board1 = game.GetBoard(game.Player1);
                var board2 = game.GetBoard(game.Player2);

                var userChoice = "";

                Console.Clear();
                var board = board2;

                if (game.NextMoveByX)
                {
                    board = board1;

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

                var x = 0;
                var y = 0;
                var bombText = "";
                while (bombText.Length < 1)
                {
                    do
                    {
                        if (game.PlayerType2 != EPlayerType.Ai || game.NextMoveByX)
                        {
                            Console.Write(
                                $"Give Y (A-{Convert.ToChar(game.Width + 64)}) X (1-{game.Height}): ");
                        }

                        (x, y) = MoveCoordinates.GetMoveCoordinates(game);
                    } while (x > game.Width - 1 || y > game.Height - 1);

                    bombText = game.MakeAMove(game, x, y, board);

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
            var game = new BattleShip();
            // var files = System.IO.Directory.EnumerateFiles(".", "*.json").ToList();
            // for (int i = 0; i < files.Count; i++)
            // {
            //     Console.WriteLine($"{i} - {files[i]}");
            // }
            using var db = new AppDbContext();
            int count = 1;
            foreach (var games in db.GameOptions)
            {
                Console.WriteLine($"{count } - {games.Name}");
                count++;
            }
            var fileNumber = Console.ReadLine();
            var fileName = "";
            count = 1;
            var gameOptionId = 0;
            foreach (var games in db.GameOptions){

                if (count == int.Parse(fileNumber))
                {
                    gameOptionId = games.GameOptionId;
                }
                count++;
            }

            // var fileName = files[int.Parse(fileNo!.Trim())];

            game = GetGame.GetGameFromDb(gameOptionId);
            // game.SetGameStateFromJsonString(jsonString);

            PlayGame(game);
            return "";
        }

        static string SaveGameAction(BattleShip game)
        {
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
