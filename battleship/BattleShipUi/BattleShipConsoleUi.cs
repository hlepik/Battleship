using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Domain;
using GameBrain;
using Console = Colorful.Console;

namespace BattleShipUi
{
    public class BattleShipConsoleUi
    {
        public static bool Hidden { get; set; } = true;

        public static void DrawBoard(CellState[,] board)
        {
            // add plus 1, since this is 0 based. length 0 is returned as -1;
            var game = new BattleShip();

            var width = board.GetUpperBound(0) + 1; // x
            var height = board.GetUpperBound(1) + 1; // y

            System.Console.WriteLine();


                for (int colIndex = 0; colIndex < width; colIndex++)
                {

                    Console.ForegroundColor = Color.DarkBlue;
                    if (colIndex == 0)
                    {
                        System.Console.Write($"    {colIndex + 1}  ");
                    }
                    else if (colIndex > 9)
                    {

                        System.Console.Write($" {colIndex + 1}  ");
                    }
                    else
                    {
                        Console.Write($"  {colIndex + 1}  ");
                    }

                    Console.ForegroundColor = Color.DimGray;


                }

                Console.WriteLine();

                for (int colIndex = 0; colIndex < width; colIndex++)
                {
                    if (colIndex == 0)
                    {

                        System.Console.Write("  ");
                    }

                    Console.Write("+•••+");
                }

                Console.WriteLine();
                char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

                for (int rowIndex = 0; rowIndex < height; rowIndex++)
                {

                    Console.ForegroundColor = Color.DarkBlue;
                    Console.Write($"{alpha[rowIndex]} ");
                    Console.ForegroundColor = Color.DimGray;
                    for (int colIndex = 0; colIndex < width; colIndex++)
                    {
                        Console.Write("| ");


                        if ( board[colIndex, rowIndex].Bomb )
                        {
                            Console.ForegroundColor = Color.Maroon;
                        }
                        else if (board[colIndex, rowIndex].Miss)
                        {
                            Console.ForegroundColor = Color.DimGray;
                        }
                        else
                        {
                            Console.ForegroundColor = Color.Blue;
                        }
                        Console.Write($"{CellString(board[colIndex, rowIndex])}");
                        Console.ForegroundColor = Color.DimGray;
                        Console.Write(" |");

                    }
                    Console.WriteLine();
                    for (int colIndex = 0; colIndex < width; colIndex++)
                    {
                        if (colIndex == 0)
                        {
                            Console.Write("  ");
                        }

                        Console.Write("+•••+");

                    }

                    Console.WriteLine();

                }

                Console.WriteLine();
                Console.ForegroundColor = Color.Blue;

        }

        public static string CellString(CellState cellState)
        {


            if (cellState.Ship)
            {
                return (Hidden ? "S" : " ")!;
            }
            if (cellState.Bomb)
            {
                return "X";
            }
            if (cellState.Miss)
            {
                return "O";
            }
            if (cellState.Empty)
            {
                return " ";
            }


            return "-";
        }

    }
}