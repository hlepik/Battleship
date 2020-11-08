using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DAL;
using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using static GameBrain.CellState;
using Console = Colorful.Console;

namespace GameBrain
{
    public class BattleShip
    {
        public static bool GameIsOver { get; set; }
        private bool _canInsert;
        public int ShipId { get; set; } = 1;
        public static EPlayerType PlayerType { get; set; }
        private bool _nextMoveByX = true;
        private EBoatsCanTouch _gameRule;
        private CellState[,] _board1 = null!;
        private CellState[,] _board2 = null!;
        public static bool Ai { get; set; }
        public static bool AiHit { get; set; }

        public List<Ship> Player1Ships { get; set; } = new List<Ship>();
        public List<Ship> Player2Ships { get; set; } = new List<Ship>();

        private int _width;
        private int _height;
        private string _player1 = null!;
        private string _player2 = null!;


        public string WhoWillPlaceTheShips { get; set; } = null!;
        public static ENextMoveAfterHit NextMove { get; set; }

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

            _width = width;
            _height = height;
            _player1 = player1;
            _player2 = player2;
            _gameRule = gameRule;
        }

        public BattleShip()
        {

        }

        public EBoatsCanTouch GetGameRule()
        {
            return _gameRule;
        }

        public CellState[,] GetBoard1()
        {
            return _board1;
        }

        public CellState[,] GetBoard2()
        {
            return _board2;
        }

        public string GetPlayer1()
        {
            return _player1;
        }

        public string GetPlayer2()
        {
            return _player2;
        }

        public int GetWidth()
        {
            return _width;
        }

        public int GetHeight()
        {
            return _height;
        }


        public CellState[,] GetBoard(string player)
        {

            if (_board1 == null)
            {
                _board1 = new CellState[_width, _height];
                _board2 = new CellState[_width, _height];
            }

            if (player == _player1)
            {
                return _board1;
            }

            return _board2;

        }


        public bool InsertBoat(int x, int y, string playerName, int size, string direction, string name)
        {

            var boardState = new BoardState
            {
                Board = new BoardSquareState[_width][]
            };
            for (int i = 0; i < _width; i++)
            {
                boardState.Board[i] = new BoardSquareState[_width];
            }

            var ships = Player1Ships;
            List<int[]> positions = new List<int[]>();
            int[] coordinates = new int[] { };
            var ship = new Ship();
            var shipId = ShipId;

            var board = _board1;
            if (playerName == _player2)
            {

                ships = Player2Ships;
                board = _board2;
            }

            if (direction == "R")
            {
                for (int i = x; i < x + size; i++)
                {

                    coordinates = new int[] {i, y};
                    board[i, y] = CellState.Ship;

                    shipId = ShipId;

                    positions.Add(coordinates);
                }

                ships.Add(new Ship(name, size, positions, direction, shipId));
                ShipId += 1;
            }
            else if (direction == "D")
            {
                for (int i = y; i < y + size; i++)
                {

                    coordinates = new int[] {x, i};
                    board[x, i] = CellState.Ship;
                    positions.Add(coordinates);
                }

                ships.Add(new Ship(name, size, positions, direction,  shipId));
                ShipId += 1;
            }
            else
            {

                coordinates = new int[] {x, y};
                board[x, y] = CellState.Ship;

                positions.Add(coordinates);
                ships.Add(new Ship(name, size, positions, direction, shipId));
                ShipId += 1;
            }

            return false;
        }



        public string MakeAMove(BattleShip game, int x, int y, CellState[,] board)
        {
            var output = "";
            if (board == _board1)
            {

                if (_board2[x, y] == Empty)
                {
                    _board2[x, y] = O;
                    _nextMoveByX = !_nextMoveByX;
                    output = "You missed!";
                }

                if (_board2[x, y] == CellState.Ship)
                {
                    _board2[x, y] = X;
                    output = BoardAfterHit(game, x, y);

                }
            }
            else
            {
                if (_board1[x, y] == Empty)
                {
                    _board1[x, y] = O;
                    _nextMoveByX = !_nextMoveByX;
                    output = "You missed!";
                }

                if (_board1[x, y] == CellState.Ship)
                {
                    _board1[x, y] = X;

                    output = BoardAfterHit(game, x, y);
                }
            }

            return output;
        }

        public int GetBoatId(string playerName, int x, int y)
        {
            var boat = new Ship();
            int[] coordinates = new int[] {x, y};
            var shipList = Player1Ships;
            var shipId = 0;
            if (playerName == _player2)
            {
                shipList = Player2Ships;
            }

            var ship = shipList
                .Where(d => d.AllPositions.Any(c => c.SequenceEqual(coordinates)));
            foreach (var each in ship)
            {
                shipId = boat.ShipId;
            }

            return shipId;
        }

        public string BoardAfterHit(BattleShip game, int x, int y)
        {
            int[] coordinates = new int[] {x, y};


            var insert = new CanInsertBoat();
            var output = "";
            var player = _player1;


            var ship = Player1Ships
                .Where(d => d.AllPositions.Any(c => c.SequenceEqual(coordinates)));

            if (NextMoveByX)
            {
                ship = Player2Ships
                    .Where(d => d.AllPositions.Any(c => c.SequenceEqual(coordinates)));
                player = _player2;
            }


            foreach (var each in ship)
            {

                if (each.Width > 1)
                {
                    output = "Hit!";
                    each.Width -= 1;
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
                    var position =each.AllPositions[0];
                    var length = each.AllPositions.Count;

                    if (Ai && !NextMoveByX)
                    {
                        AiHit = false;
                    }

                    x = position[0];
                    y = position[1];
                    insert.BoatLocationCheck(game, x, y, length, each.Direction, player);
                    GameOver();
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
            var player = _player2;
            var check = Player1Ships;
            if (NextMoveByX)
            {
                player = _player1;;
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

            var serializedBoard = new CellState[_width][];

            for (var i = 0; i < board.Length; i++)
            {
                serializedBoard [i] = new CellState[_height];
            }

            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    serializedBoard [x][y] = board[x, y];
                }
            }

            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize( serializedBoard , jsonOptions);

        }

        // public void SetGameStateFromJsonString(string jsonString)
        // {
        //     var state = JsonSerializer.Deserialize<GameState>(jsonString);
        //
        //     // restore actual state from deserialized state
        //     _player1 = state.PlayerFirst;
        //     _player2 = state.PlayerSecond;
        //     _nextMoveByX = state.NextMoveByX;
        //     _board1 =  new CellState[state.Width, state.Height];
        //     _board2 =  new CellState[state.Width, state.Height];
        //     _width = state.Width;
        //     _height = state.Height;
        //
        //     for (var x = 0; x < state.Width; x++)
        //     {
        //         for (var y = 0; y < state.Height; y++)
        //         {
        //             _board1[x, y] = state.Board1[x][y];
        //             _board2[x, y] = state.Board2[x][y];
        //         }
        //     }
        //
        // }

        public void SHipId()
        {
            var boardState = new BoardState();
            var player = "";
            var hasBomb = false;
            var shipId = 0;
            var board = _board2;
            if (_nextMoveByX)
            {
                player = _player1;
                board = _board1;
            }
            else
            {
                player = _player2;
            }

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if (board[i, j] == CellState.Ship)
                    {
                        shipId = GetBoatId(player, i, j);
                    }

                    if (board[i, j] == X)
                    {
                        hasBomb = true;
                    }
                    boardState.Board[i][j] = new BoardSquareState()
                    {
                        BoatId = shipId,
                        Bomb = hasBomb
                    };

                }
            }
        }

        public void SaveGameToDb()
        {

            using var db = new AppDbContext();
            // db.Database.Migrate();

            var playerA = new Player()
            {
                Name = _player1,
                EPlayerType = PlayerType,
                Game = new Game(),
                GameBoats = new List<GameBoat>(),
                PlayerBoardStates = new List<PlayerBoardState>()
            };

            var playerB = new Player()
            {
                Name = _player2,
                EPlayerType = PlayerType,
                Game = new Game(),
                GameBoats = new List<GameBoat>(),
                PlayerBoardStates = new List<PlayerBoardState>()
            };


            var game = new Game()
            {
                Description = DateTime.Now.ToLongDateString()
            };
            game.PlayerA = playerA;
            game.PlayerB = playerB;
            db.Players.Add(game.PlayerA);
            db.Players.Add(game.PlayerB);

            var gameOption = new GameOption()
            {
                Name = "Battleship",
                BoardWidth = _width,
                BoardHeight = _height,
                EBoatsCanTouch = _gameRule,
                ENextMoveAfterHit = NextMove,
                Games = new List<Game>(),
                GameOptionBoats = new List<GameOptionBoat>()

            };
            game.GameOption = gameOption;
            db.GameOptions.Add(game.GameOption);

            var gameBoat1 = new GameBoat();
            foreach (var each in Player1Ships)
            {
                gameBoat1 = new GameBoat()
                {
                    Name = each.Name,
                    Size = each.Width,
                    IsSunken = each.IsSunken,
                    Direction = each.Direction,
                    // Coordinates = each.AllPositions,
                    Player = playerA
                };
            }
            db.GameBoats.Add(gameBoat1);
            var gameBoat2 = new GameBoat();
            foreach (var each in Player2Ships)
            {
                gameBoat2 = new GameBoat()
                {
                    Name = each.Name,
                    Size = each.Width,
                    IsSunken = each.IsSunken,
                    Direction = each.Direction,
                    // Coordinates = each.AllPositions,
                    Player = playerB
                };
            }
            db.GameBoats.Add(gameBoat2);

            var ship = new Ship();
            var boat = new Boat()
            {
                Name =  ship.Name,
                Size =  ship.Width,
                GameOptionBoats = new List<GameOptionBoat>()
            };
            var playerBoardState = new PlayerBoardState()
            {
                BoardState = BoardToJson(_board1)
            };

            var boardState = new BoardState();
            var player = "";
            var hasBomb = false;
            var shipId = 0;
            var board = _board2;
            if (_nextMoveByX)
            {
                player = _player1;
                board = _board1;
            }
            else
            {
                player = _player2;
            }

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if (board[i, j] == CellState.Ship)
                    {
                        shipId = GetBoatId(player, i, j);
                    }

                    if (board[i, j] == X)
                    {
                        hasBomb = true;
                    }
                    boardState.Board[i][j] = new BoardSquareState()
                    {
                        BoatId = shipId,
                        Bomb = hasBomb
                    };

                }
            }


            db.Boats.Add(boat);

            Console.WriteLine("Before add");
            Console.WriteLine(game);
            Console.WriteLine(gameBoat1);
            // db.Games.Add(game);
            // // this will actually save data to db
            // db.SaveChanges();



        }
        public string BoardToJson(CellState[,] board)
        {

            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            var serializedGame = GetSerializedGameState(board);
            var jsonString = JsonSerializer.Serialize(serializedGame, jsonOptions);

            return jsonString;

        }


    }

}