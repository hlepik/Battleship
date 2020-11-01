using System;
using System.Globalization;
using Domain11.Enums;

namespace BattleShipUi
{
    public class NextMoveAfterHit
    {
        public void NextMoveAfterHitRule()
        {
            Console.WriteLine("Can player make a next move after hit? "+ Environment.NewLine +
                              "Y - Yes" + Environment.NewLine +
                              "N - No");

            var input = Console.ReadLine().Trim().ToUpper();

            input = input switch
            {
                "Y" => ENextMoveAfterHit.SamePlayer.ToString(),
                "N" => ENextMoveAfterHit.OtherPlayer.ToString(),
            };
            }

    }
}