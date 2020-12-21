using System;
using System.Drawing;
using Domain.Enums;
using Console = Colorful.Console;

namespace BattleShipUi
{
    public class NextMoveAfterHit
    {
        public  ENextMoveAfterHit NextMoveAfterHitRule()
        {
            var nextMove = ENextMoveAfterHit.SamePlayer;
            var input = "";
            do
            {
                Console.WriteLine("Can player make a next move after hit? " + Environment.NewLine +
                                  "Y - Yes" + Environment.NewLine +
                                  "N - No");

                input = Console.ReadLine().Trim().ToUpper();

                if (input != "Y" && input != "N")
                {
                    Console.ForegroundColor = Color.Maroon;
                    Console.WriteLine("Invalid input!");
                    Console.ForegroundColor = Color.Blue;
                }
            } while (input != "Y" && input != "N");

            nextMove = input switch
            {
                "Y" => ENextMoveAfterHit.SamePlayer,
                "N" => ENextMoveAfterHit.OtherPlayer,
                _ => nextMove
            };

            return nextMove;
        }
    }
}