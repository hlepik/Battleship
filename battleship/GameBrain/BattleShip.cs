using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DAL;
using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Console = Colorful.Console;

namespace GameBrain
{
    public class BattleShip
    {
        public static bool GameIsOver { get; set; }
        private bool _canInsert;
        public int ShipId { get; set; } = 1;
        public  EPlayerType PlayerType1 { get; set; }
        public  EPlayerType PlayerType2 { get; set; }
        private bool _nextMoveByX = true;
        public EBoatsCanTouch GameRule;
        public CellState[,] Board1  { get; set; } = default! ;
        public CellState[,] Board2  { get; set; } = default!;

        public static bool Ai { get; set; }
        public bool AiHit { get; set; }

        public List<Ship> Player1Ships { get; set; } = new List<Ship>();
        public List<Ship> Player2Ships { get; set; } = new List<Ship>();

        public int Width;
        public int Height;
        public string Player1 = null!;
        public string Player2 = null!;
        public string FileName = null!;


        public string WhoWillPlaceTheShips { get; set; } = null!;
        public ENextMoveAfterHit NextMove { get; set; }

        public bool NextMoveByX
        {
            get => _nextMoveByX;
            set => _nextMoveByX = value;
        }

        public bool CanInsert
        {
            get => _canInsert;
            set => _canInsert = value;
        }

        public BattleShip(int width, int height, string player1, string player2, EBoatsCanTouch gameRule)
        {

            Width = width;
            Height = height;
            Player1 = player1;
            Player2 = player2;
            GameRule = gameRule;
        }

        public BattleShip()
        {

        }

        public CellState[,] GetBoard(string player)
        {

            if (Board1 == null)
            {
                Board1 = new CellState[Width, Height];
                Board2 = new CellState[Width, Height];

                for (var row = 0; row < Width; row++)
                {
                    for (var col = 0; col < Height; col++)
                    {
                        Board1[row, col] = new CellState();
                        Board2[row, col] = new CellState();
                    }
                }
            }

            if (player == Player1)
            {
                return Board1;
            }

            return Board2;

        }


        public bool InsertBoat(int x, int y, string playerName, int size, string direction, string name)
        {

            var ships = Player1Ships;
            var shipId = ShipId;

            var board = Board1;
            if (playerName == Player2)
            {
                ships = Player2Ships;
                board = Board2;
            }

            if (direction == "R")
            {
                for (int i = x; i < x + size; i++)
                {
                    board[i, y].Empty = false;
                    board[i, y].Ship = true;
                    board[i, y].ShipId = ShipId;
                }

                board[x, y].FirstLocation = new[] {x, y};
                ships.Add(new Ship(name, size, direction, shipId));
                ShipId += 1;
            }
            else if (direction == "D")
            {
                for (int i = y; i < y + size; i++)
                {
                    board[x, i].Empty = false;
                    board[x, i].Ship = true;
                    board[x, i].ShipId = ShipId;

                }
                board[x, y].FirstLocation = new[] {x, y};
                ships.Add(new Ship(name, size, direction, shipId));

                ShipId += 1;
            }
            else
            {
                board[x, y].Empty = false;
                board[x, y].Ship = true;
                board[x, y].ShipId = ShipId;

                ships.Add(new Ship(name, size, direction, shipId));
                ShipId += 1;
            }

            return false;
        }

        public string MakeAMove(BattleShip game, int x, int y, CellState[,] board)
        {
            var output = "";

            if (board == Board1)
            {

                if (Board2[x, y].Ship)
                {
                    Board2[x, y].Ship = false;
                    Board2[x, y].Bomb = true;
                    output = BoardAfterHit(game, x, y);

                }
                else if (Board2[x, y].Empty)
                {
                    Board2[x, y].Empty = false;
                    Board2[x, y].Miss = true;
                    _nextMoveByX = !_nextMoveByX;
                    output = "You missed!";
                }
            }
            else
            {
                if (Board1[x, y].Ship)
                {
                    Board1[x, y].Ship = false;
                    Board1[x, y].Bomb = true;

                    output = BoardAfterHit(game, x, y);
                }
                else if (Board1[x, y].Empty)
                {
                    Board1[x, y].Empty = false;
                    Board1[x, y].Miss = true;
                    _nextMoveByX = !_nextMoveByX;
                    output = "You missed!";
                }


            }

            return output;
        }

        public string BoardAfterHit(BattleShip game, int x, int y)
        {

            var ship = Player1Ships;
            var insert = new CanInsertBoat();
            var output = "";
            var player = Player1;
            var board = Board1;
            if (NextMoveByX)
            {
                player = Player2;
                board = Board2;
                ship = Player2Ships;
            }

            var id = board[x, y].ShipId;

            foreach (var each in ship)
            {

                if (each.ShipId == id)
                {

                    if (each.LifeCount > 1)
                    {
                        output = "Hit!";
                        each.LifeCount -= 1;

                        if (Ai && !NextMoveByX)
                        {
                            AiHit = true;
                        }
                    }
                    else
                    {
                        output = "Ship has been destroyed!";
                        each.IsSunken = true;
                        insert.AroundShip = true;

                        if (Ai && !NextMoveByX)
                        {
                            AiHit = false;
                        }
                        var xy = board[x, y].FirstLocation;
                        x = xy[0];
                        y = xy[1];


                        insert.BoatLocationCheck(game, x, y, each.Width, each.Direction, player);
                        GameOver();
                    }

                }

                if (ENextMoveAfterHit.OtherPlayer == NextMove)
                {
                    game.NextMoveByX = !game.NextMoveByX;
                }
            }

            return output;
        }



        public bool  GameOver()
        {

            var count = 0;
            var player = Player2;
            var check = Player1Ships;
            if (NextMoveByX)
            {
                check = Player2Ships;
                player = Player1;;
            }
            foreach (var each in check)
            {
                if (each.IsSunken)
                {
                    count++;
                }

            }
            if (count == check.Count)
            {
                GameIsOver = true;
                return true;

            }

            return false;
        }


        public string GetSerializedGameState(CellState[,] board)
        {

            var state = new GameState
            {
                NextMoveByX = _nextMoveByX,
                Width = Width,
                Height = Height,

            };

            state.Board1 = new CellState[state.Width ][];

            for (var i = 0; i < state.Board1.Length; i++)
            {
                state.Board1[i] = new CellState[state.Height];
            }


            for (var x = 0; x < state.Width; x++)
            {
                for (var y = 0; y < state.Height; y++)
                {
                    state.Board1[x][y] = board[x, y];

                }
            }

            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(state, jsonOptions);

        }


        public string SaveGameToDb()
        {
            using var db = new AppDbContext();
            db.Database.Migrate();


            var gameOption = new GameOption()
            {
                Name = FileName,
                BoardWidth = Width,
                BoardHeight = Height,
                EBoatsCanTouch = GameRule,
                ENextMoveAfterHit = NextMove,
            };
            db.GameOptions.Add(gameOption);
            var playerA = new Player()
            {
                Name = Player1,
                EPlayerType = PlayerType1,
            };

            var playerB = new Player()
            {
                Name = Player2,
                EPlayerType = PlayerType2,
            };

            var game = new Game();
            game.PlayerA = playerA;
            game.PlayerB = playerB;


            game.GameOption = gameOption;
            db.Games.Add(game);
            db.SaveChanges();

            playerA.Game = game;
            playerB.Game = game;
            foreach (var each in Player1Ships)
            {
                var boat = new Boat()
                {
                    Name = each.Name,
                    Size = each.Width
                };
                db.Boats.Add(boat);
            }

            foreach (var each in Player2Ships)
            {
                var gameBoat = new GameBoat()
                {
                    Name = each.Name,
                    Size = each.Width,
                    IsSunken = each.IsSunken,
                    Player = playerB,
                    Direction = each.Direction,
                    LifeCount = each.LifeCount
                };
                gameBoat.Player = playerB;
                db.GameBoats.Add(gameBoat);
            }


            foreach (var each in Player1Ships)
            {
                var gameBoat = new GameBoat()
                {
                    Name = each.Name,
                    Size = each.Width,
                    IsSunken = each.IsSunken,
                    Player = playerB,
                    Direction = each.Direction,
                    LifeCount = each.LifeCount
                };
                gameBoat.Player = playerA;
                db.GameBoats.Add(gameBoat);
            }

            var playerABoardState = new PlayerBoardState()
            {
                BoardState =  GetSerializedGameState(Board1),
                Player = playerA
            };
            var playerBBoardState = new PlayerBoardState()
            {
                BoardState =  GetSerializedGameState(Board2),
                Player = playerB
            };

            db.PlayerBoardStates.Add(playerABoardState);
            db.PlayerBoardStates.Add(playerBBoardState);
            // // this will actually save data to db

            db.SaveChanges();
            return "";
        }


    }

}