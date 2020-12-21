using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DAL;
using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GameBrain
{
    public class BattleShip
    {
        public static bool GameIsOver { get; set; }
        private bool _canInsert;
        public Game? Game { get; set; }
        public int GameId { get; set; }
        public int ShipId { get; set; } = 1;
        public  EPlayerType PlayerType1 { get; set; }
        public  EPlayerType PlayerType2 { get; set; }
        private bool _nextMoveByX = true;
        public EBoatsCanTouch GameRule;
        public CellState[,] Board1  { get; set; } = default! ;
        public CellState[,] Board2  { get; set; } = default!;
        public bool TextWhenMiss { get; set; }
        public bool TextWhenHit { get; set; }

        public static bool Ai { get; set; }

        public List<Ship> Player1Ships { get; set; } = new List<Ship>();
        public List<Ship> Player2Ships { get; set; } = new List<Ship>();

        public int Width;
        public int Height;

        public string Player1 = null!;
        public string Player2 = null!;
        public string FileName = null!;
        public int PlayerAId { get; set; }
        public int PlayerBId { get; set; }


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
                    board[i, y].FirstLocation = new[] {x, y};
                }

                ships.Add(new Ship(name, size, direction, ShipId, false, size));
                ShipId += 1;
            }
            else if (direction == "D")
            {
                for (int i = y; i < y + size; i++)
                {
                    board[x, i].Empty = false;
                    board[x, i].Ship = true;
                    board[x, i].ShipId = ShipId;
                    board[x, i].FirstLocation = new[] {x, y};
                }
                ships.Add(new Ship(name, size, direction, ShipId, false, size));

                ShipId += 1;
            }
            else
            {
                board[x, y].Empty = false;
                board[x, y].Ship = true;
                board[x, y].ShipId = ShipId;

                ships.Add(new Ship(name, size, direction, ShipId, false, size));
                ShipId += 1;
            }

            return false;
        }

        public string MakeAMove(int x, int y, CellState[,] board, BattleShip game)
        {

            TextWhenMiss = false;

            if (board[x, y].Ship)
            {
               board[x, y].Ship = false;
               board[x, y].Bomb = true;
               BoardAfterHit(x, y, game);

            }
            else if (board[x, y].Empty)
            {
                board[x, y].Empty = false;
                board[x, y].Miss = true;
                _nextMoveByX = !_nextMoveByX;
                TextWhenMiss = true;
            }


            return "";
        }

        public string BoardAfterHit(int x, int y, BattleShip game)
        {

            TextWhenHit = false;
            var ship = Player1Ships;
            var insert = new CanInsertBoat();

            var playerName = Player1;
            var board = Board1;
            if (NextMoveByX)
            {
                playerName = Player2;
                board = Board2;
                ship = Player2Ships;
            }

            ShipId = board[x, y].ShipId;

            foreach (var each in ship)
            {

                if (each.ShipId == ShipId)
                {

                    if (each.LifeCount > 1)
                    {
                        TextWhenHit = true;
                        each.LifeCount -= 1;

                    }
                    else
                    {
                        each.LifeCount -= 1;
                        each.IsSunken = true;
                        insert.AroundShip = true;

                        var xy = board[x, y].FirstLocation;
                        x = xy[0];
                        y = xy[1];



                        insert.BoatLocationCheck(game, x, y, each.Width, each.Direction, playerName);
                        GameOver();
                    }
                }
            }

            if (ENextMoveAfterHit.OtherPlayer == NextMove)
            {
                NextMoveByX = !NextMoveByX;
            }
            return "";
        }

        public bool  GameOver()
        {

            var count = 0;

            var check = Player1Ships;
            if (NextMoveByX)
            {
                check = Player2Ships;
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

            var state = new GameState()
            {
                Width = Width,
                Height = Height,
            };

            state.Board = new CellState[state.Width ][];

            for (var i = 0; i < state.Board.Length; i++)
            {
                state.Board[i] = new CellState[state.Height];
            }

            for (var x = 0; x < state.Width; x++)
            {
                for (var y = 0; y < state.Height; y++)
                {
                    state.Board[x][y] = board[x, y];
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


            if (GameId != 0)
            {
                Game = db.Games!.Where(x => x.GameId == GameId)
                    .Include(x => x.PlayerAId)
                    .Include(x => x.PlayerBId).FirstOrDefault();

                if (NextMoveByX)
                {
                    var player2BoardState = new PlayerBoardState()
                    {
                        BoardState =  GetSerializedGameState(Board2),
                        Player = Game!.PlayerB
                    };
                    db.PlayerBoardStates.Add(player2BoardState);

                    foreach (var each in Player2Ships)
                    {
                        var gameBoat = new GameBoat()
                        {
                            Name = each.Name,
                            Size = each.Width,
                            IsSunken = each.IsSunken,
                            Direction = each.Direction,
                            LifeCount = each.LifeCount,
                            ShipId = each.ShipId
                        };
                        gameBoat.Player = Game.PlayerB;
                        db.GameBoats!.Add(gameBoat);
                    }
                }
                else
                {

                    var player1BoardState = new PlayerBoardState()
                    {
                        BoardState = GetSerializedGameState(Board1),
                        Player = Game!.PlayerA
                    };

                    db.PlayerBoardStates.Add(player1BoardState);



                    foreach (var each in Player1Ships)
                    {
                        var gameBoat = new GameBoat()
                        {
                            Name = each.Name,
                            Size = each.Width,
                            IsSunken = each.IsSunken,
                            Direction = each.Direction,
                            LifeCount = each.LifeCount,
                            ShipId = each.ShipId
                        };
                        gameBoat.Player = Game.PlayerA;
                        db.GameBoats!.Add(gameBoat);
                    }
                }

                db.SaveChanges();

                return "";
            }

            var gameOption = new GameOption()
            {
                Name = FileName,
                BoardWidth = Width,
                BoardHeight = Height,
                EBoatsCanTouch = GameRule,
                ENextMoveAfterHit = NextMove
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

            var game = new Game()
            {
                Date = DateTime.Now.ToString(),
                NextMoveByX = NextMoveByX
            };
            game.PlayerA = playerA;
            game.PlayerB = playerB;


            game.GameOption = gameOption;
            db.Games!.Add(game);
            db.SaveChanges();

            GameId = game.GameId;

            foreach (var each in Player2Ships)
            {
                var gameBoat = new GameBoat()
                {
                    Name = each.Name,
                    Size = each.Width,
                    IsSunken = each.IsSunken,
                    Direction = each.Direction,
                    LifeCount = each.LifeCount,
                    ShipId = each.ShipId,
                    IsInserted = true

                };
                gameBoat.Player = playerB;
                db.GameBoats!.Add(gameBoat);
            }


            foreach (var each in Player1Ships)
            {
                var gameBoat = new GameBoat()
                {
                    Name = each.Name,
                    Size = each.Width,
                    IsSunken = each.IsSunken,
                    Direction = each.Direction,
                    LifeCount = each.LifeCount,
                    ShipId = each.ShipId,
                    IsInserted = true
                };
                gameBoat.Player = playerA;
                db.GameBoats!.Add(gameBoat);
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

        public string SetGameStateFromJsonString(string board1, string player)
        {
            var state = JsonSerializer.Deserialize<GameState>(board1);

            // restore actual state from deserialized state


            if (state != null)
            {
                if (player == Player1)
                {
                    Board1 = new CellState[state.Width, state.Height];

                    for (var x = 0; x < state.Width; x++)
                    {
                        for (var y = 0; y < state.Height; y++)
                        {
                            Board1[x, y] = state.Board[x][y];
                        }
                    }
                }
                else
                {
                    Board2 = new CellState[state.Width, state.Height];

                    for (var x = 0; x < state.Width; x++)
                    {
                        for (var y = 0; y < state.Height; y++)
                        {
                            Board2[x, y] = state.Board[x][y];
                        }
                    }
                }
            }

            return "";

        }

        public CellState GetCell(int x, int y, CellState[,] board)
        {
            return board[x, y];
        }

    }

}