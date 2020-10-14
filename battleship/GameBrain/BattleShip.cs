using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Text.Json;
using Console = Colorful.Console;

namespace GameBrain
{
    public class BattleShip
    {

        public static CellState[,]? Board1;
        public static CellState[,]? Board2;
        private CellState[,] _board1 = null!;
        private CellState[,] _board2 = null!;
        public static int Width;
        public static int Height;

        public static string Player1 = null!;
        public static string Player2 = null!;

        public static bool NextMoveByX = true;

        public CellState[,] GetBoard()
        {

            if (_board1 == null)
            {
                _board1 = new CellState[Width,Height];
                _board2 = new CellState[Width,Height];
            }

            if (NextMoveByX)
            {
                return _board1;
            }

            return _board2;
        }


        public bool MakeAMove(int x, int y, CellState[,] board)
        {

            Board2 = _board2;
            Board1 = _board1;
            if (board == Board1)
            {

                if(Board2[x, y] == CellState.Empty)
                {
                    Board2[x, y] = CellState.X;
                    NextMoveByX = !NextMoveByX;

                    return true;
                }
            }
            else
            {
                if(Board1[x, y] == CellState.Empty)
                {
                    Board1[x, y] = CellState.X;
                    NextMoveByX = !NextMoveByX;

                    return true;
                }
            }

            return false;
        }
        public string GetSerializedGameState()
        {
            var state = new GameState
            {
                NextMoveByX = NextMoveByX,
                Width = _board1.GetLength(0),
                Height =_board1.GetLength(0),
                PlayerFirst = Player1,
                PlayerSecond = Player2
            };

            state.Board2 = new CellState[state.Width ][];

            for (var i = 0; i < state.Board2.Length; i++)
            {
                state.Board2[i] = new CellState[state.Height];
            }
            state.Board1 = new CellState[state.Width][];
            for (var i = 0; i < state.Board1.Length; i++)
            {
                state.Board1[i] = new CellState[state.Height];
            }

            for (var x = 0; x < state.Width; x++)
            {
                for (var y = 0; y < state.Height; y++)
                {
                    state.Board1[x][y] = _board1[x, y];
                    state.Board2[x][y] = _board2[x, y];
                }
            }

            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(state, jsonOptions);

        }

        public void SetGameStateFromJsonString(string jsonString)
        {
            var state = JsonSerializer.Deserialize<GameState>(jsonString);

            // restore actual state from deserialized state
            NextMoveByX = state.NextMoveByX;
            _board1 =  new CellState[state.Width, state.Height];
            _board2 =  new CellState[state.Width, state.Height];

            for (var x = 0; x < state.Width; x++)
            {
                for (var y = 0; y < state.Height; y++)
                {
                    _board1[x, y] = state.Board1[x][y];
                    _board2[x, y] = state.Board2[x][y];
                }
            }

        }
    }

}