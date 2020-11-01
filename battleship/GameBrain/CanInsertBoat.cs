using System;
using System.Data;
using Domain;
using Domain.Enums;

namespace GameBrain
{
    public class CanInsertBoat
    {
        public bool GameAPlacement(BattleShip game, int x, int y, int size, string direction, string playerName)
        {

            var board = game.GetBoard1();
            if (playerName == game.GetPlayer2())
            {
                board = game.GetBoard2();
            }
            switch (direction)
            {
                case "R" when EBoatsCanTouch.Yes.ToString() == game.GetGameRule():
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
                            if (EBoatsCanTouch.No.ToString() == game.GetGameRule() && board[i, j] != CellState.Empty)
                            {
                                game.CanInsert = false;
                                break;
                            }

                            if (EBoatsCanTouch.Corner.ToString() == game.GetGameRule() &&
                                board[i, j] != CellState.Empty)
                            {
                                game.CanInsert = false;

                                if (i == x -1 && j== y -1)
                                {
                                    game.CanInsert = true;

                                }
                                else if (i == x - 1 && j== y + 1)
                                {
                                    game.CanInsert = true;
                                }
                                else if (i == x + size && j == y + 1)
                                {
                                    game.CanInsert = true;
                                }
                                else if (i == x + size && j == y -1)
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
            case "D" when EBoatsCanTouch.Yes.ToString() == game.GetGameRule():
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
                        if (EBoatsCanTouch.No.ToString() == game.GetGameRule() && board[i, j] != CellState.Empty)
                        {
                            game.CanInsert = false;
                            break;
                        }

                        if (EBoatsCanTouch.Corner.ToString() == game.GetGameRule() &&
                            board[i, j] != CellState.Empty)
                        {
                            game.CanInsert = false;

                            if (i == x -1 && j== y -1)
                            {
                                game.CanInsert = true;

                            }
                            else if (i == x - 1 && j== y + size)
                            {
                                game.CanInsert = true;
                            }
                            else if (i == x + 1 && j == y + size)
                            {
                                game.CanInsert = true;
                            }
                            else if (i == x + 1 && j == y -1)
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
            case "D":
                game.CanInsert = false;
                break;
            case "":
            {
                for (int i = x - 1; i < x + 2; i++)
                {
                    for (int j = y - 1; j < y + 2; j++)
                    {
                        if (i < 0 || i >= game.GetWidth() || j < 0 || j >= game.GetHeight()) continue;
                        if (EBoatsCanTouch.No.ToString() == game.GetGameRule() && board[i, j] != CellState.Empty)
                        {
                            game.CanInsert = false;
                            break;
                        }

                        if (EBoatsCanTouch.Corner.ToString() == game.GetGameRule() &&
                            board[i, j] != CellState.Empty)
                        {
                            game.CanInsert = false;

                            if (i == x -1 && j== y -1)
                            {
                                game.CanInsert = true;

                            }
                            else if (i == x - 1 && j== y + 1)
                            {
                                game.CanInsert = true;
                            }
                            else if (i == x + 1 && j == y + 1)
                            {
                                game.CanInsert = true;
                            }
                            else if (i == x + 1 && j == y -1)
                            {
                                game.CanInsert = true;
                            }
                        }

                        if (game.CanInsert) continue;
                        return false;
                    }
                }
                break;
            }
        }
            return true;
        }
    }
}