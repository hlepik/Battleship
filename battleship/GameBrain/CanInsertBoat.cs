using System;
using System.Data;
using Domain;
using Domain.Enums;
using static Domain.Enums.EBoatsCanTouch;

namespace GameBrain
{
    public class CanInsertBoat
    {
        private bool _aroundShip;

        public bool AroundShip
        {
            get => _aroundShip;
            set => _aroundShip = value;
        }

        public bool BoatLocationCheck(BattleShip game, int x, int y,  int size, string direction, string playerName)
        {

            game.CanInsert = true;

            var board = game.GetBoard1();

            if (playerName == game.GetPlayer2())
            {
                board = game.GetBoard2();
            }

            switch (direction)
            {
                case "R" when Yes == game.GetGameRule():
                {
                    for (int i = x; i < x + size; i++)
                    {
                        if (i >= game.GetWidth() || board[i, y] != CellState.Empty)
                        {
                            game.CanInsert = false;
                            break;
                        }

                    }

                    break;
                }
                case "R" when x + size -1 < game.GetWidth():
                {
                    for (int i = x - 1; i < x + size + 1; i++)
                    {
                        for (int j = y - 1; j < y + 2; j++)
                        {
                            if (i < 0 || i >= game.GetWidth() || j < 0 || j >= game.GetHeight()) continue;
                            if (No== game.GetGameRule() && board[i, j] != CellState.Empty && !_aroundShip)
                            {
                                game.CanInsert  = false;
                                break;
                            }
                            if (No == game.GetGameRule() && board[i, j] == CellState.Empty && _aroundShip)
                            {
                                board[i, j] = CellState.O;
                            }
                            if (Corner == game.GetGameRule() &&
                                board[i, j] == CellState.Empty && _aroundShip)
                            {
                                if (!BoardCorner(x, y, i, j, size, direction))
                                {
                                    board[i, j] = CellState.O;
                                }

                            }

                            if (Corner == game.GetGameRule() &&
                                board[i, j] != CellState.Empty && !_aroundShip)
                            {
                                game.CanInsert = false;

                                if (BoardCorner(x, y, i, j, size, direction))
                                {
                                    game.CanInsert = true;
                                }
                            }

                            if (game.CanInsert) continue;
                            game.CanInsert = false;
                            break;
                        }


                        if (game.CanInsert) continue;
                        game.CanInsert = false;
                        break;
                    }

                    break;
                }
            case "R":
                game.CanInsert = false;
                break;
            case "D" when Yes == game.GetGameRule():
            {
                for (int i = y; i < y + size; i++)
                {
                    if (i >= game.GetHeight() || board[x, i] != CellState.Empty)
                    {
                        game.CanInsert = false;
                        break;
                    }
                }

                break;
            }
            case "D" when y + size -1 < game.GetHeight():
            {
                for (int i = x - 1; i < x + 2; i++)
                {
                    for (int j = y - 1; j < y + size + 1; j++)
                    {
                        if (i < 0 || i >= game.GetWidth() || j < 0 || j >= game.GetHeight()) continue;
                        if (No == game.GetGameRule() && board[i, j] != CellState.Empty && !_aroundShip)
                        {
                            game.CanInsert = false;
                            break;
                        }
                        if (No == game.GetGameRule() && board[i, j] == CellState.Empty && _aroundShip)
                        {
                            board[i, j] = CellState.O;
                        }
                        if (Corner == game.GetGameRule() &&
                            board[i, j] == CellState.Empty && _aroundShip)
                        {
                            if (!BoardCorner(x, y, i, j, size, direction))
                            {
                                board[i, j] = CellState.O;
                            }

                        }

                        if (Corner == game.GetGameRule() &&
                            board[i, j] != CellState.Empty && !_aroundShip)
                        {
                            game.CanInsert = false;

                            if (BoardCorner(x, y, i, j, size, direction))
                            {
                                game.CanInsert = true;
                            }
                        }

                        if (game.CanInsert ) continue;
                        game.CanInsert = false;
                        break;
                    }

                    if (game.CanInsert) continue;
                    game.CanInsert = false;
                    break;
                }

                break;
            }
            case "D":
                game.CanInsert = false;
                break;

        }
            return true;
        }

        public bool BoardCorner(int x, int y, int i, int j, int size, string direction)
        {
            var corner = false;

            if (direction == "R")
            {
                if (i == x - 1 && j == y - 1)
                {
                    corner = true;

                }
                else if (i == x - 1 && j == y + 1)
                {
                    corner = true;
                }
                else if (i == x + size && j == y + 1)
                {
                    corner = true;
                }
                else if (i == x + size && j == y - 1)
                {
                    corner = true;
                }

                if (corner)
                {
                    return true;
                }
            }

            if (direction == "D")
            {

                if (i == x - 1 && j == y - 1)
                {
                    corner = true;

                }
                else if (i == x - 1 && j == y + 1)
                {
                    corner = true;
                }
                else if (i == x + 1 && j == y + size)
                {
                    corner = true;
                }
                else if (i == x - 1 && j == y + size)
                {
                    corner = true;
                }

                if (corner)
                {
                    return true;
                }
            }

            return false;
        }

    }
}