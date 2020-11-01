using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using GameBrain;
using Console = Colorful.Console;

namespace BattleShipUi
{
    public class BattleShipConsoleUi
    {
        public static void DrawBoard(CellState[,] board)
        {
            // add plus 1, since this is 0 based. length 0 is returned as -1;

            var width = board.GetUpperBound(0) + 1; // x
            var height = board.GetUpperBound(1) + 1; // y

            System.Console.WriteLine();
            for (int colIndex = 0; colIndex < width; colIndex++)
            {
                Console.ForegroundColor = Color.DarkBlue;
                if (colIndex ==0)
                {
                    System.Console.Write($"    {colIndex + 1}  ");
                }else if (colIndex > 9)
                {
                    System.Console.Write($" {colIndex + 1}  ");
                }
                else
                {
                    Console.Write($"  {colIndex + 1}  ");
                }

                Console.ForegroundColor = Color.Blue;


            }
            Console.WriteLine();


            for (int colIndex = 0; colIndex < width; colIndex++)
            {
                if (colIndex ==0)
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
                Console.ForegroundColor = Color.Blue;
                for (int colIndex = 0; colIndex < width; colIndex++)
                {

                    Console.Write($"| {CellString(board[colIndex, rowIndex])} |");

                }


                Console.WriteLine();
                for (int colIndex = 0; colIndex < width; colIndex++)
                {
                    if (colIndex==0)
                    {
                        System.Console.Write("  ");
                    }
                    Console.Write("+•••+");

                }
                Console.WriteLine();

            }
            Console.WriteLine();

        }

        public static string CellString(CellState cellState)
        {
            switch (cellState)
            {
                case CellState.Empty: return " ";
                case CellState.Ship: return "S";
                case CellState.X: return "X";
                case CellState.O: return "O";
            }

            return "-";
        }


    }
}