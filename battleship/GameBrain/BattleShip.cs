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
        private bool _nextMoveByX = true;
        private CellState[,] _board1 = null!;
        private CellState[,] _board2 = null!;
        private int _width;
        private int _height;
        private string _player1 = null!;
        private string _player2 = null!;
        public bool NextMoveByX => _nextMoveByX;


        public BattleShip(int width, int height, string player1, string player2)
        {
            _player1 = player1;
            _player2 = player2;
            _width = width;
            _height = height;

        }
        public BattleShip()
        {

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

        public bool MakeAMove(int x, int y, CellState[,] board)
        {

            if (board == _board1)
            {

                if(_board2[x, y] == CellState.Empty)
                {
                   _board2[x, y] = CellState.X;
                    _nextMoveByX = !_nextMoveByX;
                    return true;
                }
            }
            else
            {
                if(_board1[x, y] == CellState.Empty)
                {
                   _board1[x, y] = CellState.X;
                    _nextMoveByX = !_nextMoveByX;

                    return true;
                }
            }
            return false;
        }

        public string GetSerializedGameState()
        {
            var state = new GameState
            {
                NextMoveByX = _nextMoveByX,
                Width = _width,
                Height = _height,
                PlayerFirst = _player1,
                PlayerSecond = _player2
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
            _player1 = state.PlayerFirst;
            _player2 = state.PlayerSecond;
            _nextMoveByX = state.NextMoveByX;
            _board1 =  new CellState[state.Width, state.Height];
            _board2 =  new CellState[state.Width, state.Height];
            _width = state.Width;
            _height = state.Height;

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