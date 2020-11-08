using System;
using System.Collections.Generic;

namespace GameBrain
{
    public class BoatCount
    {
        public List<Ship> BoatsCount(BattleShip game)
        {

            var size = BoatSizes.CalculateBoats(game);
            List<Ship> boats = new List<Ship>();

            boats.Add(new Patrol());
            boats.Add(new Patrol());
            boats.Add(new Cruiser());

            if (size >= 3)
            {
                boats.Add(new Patrol());
                boats.Add(new Cruiser());
                boats.Add(new Submarine());
            }
            if (size >= 4)
            {
                boats.Add(new Patrol());
                boats.Add(new Cruiser());
                boats.Add(new Submarine());
                boats.Add(new Battleship());
            }
            if (size >= 5)
            {
                boats.Add(new Patrol());
                boats.Add(new Cruiser());
                boats.Add(new Submarine());
                boats.Add(new Battleship());
                boats.Add(new Carrier());
            }
            if (size >= 6)
            {
                boats.Add(new Patrol());
                boats.Add(new Cruiser());
                boats.Add(new Submarine());
                boats.Add(new Battleship());
                boats.Add(new Carrier());
                boats.Add(new MegaSubmarine());
            }
            if (size >= 7)
            {
                boats.Add(new Patrol());
                boats.Add(new Cruiser());
                boats.Add(new Submarine());
                boats.Add(new Battleship());
                boats.Add(new Carrier());
                boats.Add(new MegaSubmarine());
                boats.Add(new MegaBattleship());
            }
            if (size >= 8)
            {
                boats.Add(new Patrol());
                boats.Add(new Cruiser());
                boats.Add(new Submarine());
                boats.Add(new Battleship());
                boats.Add(new Carrier());
                boats.Add(new MegaSubmarine());
                boats.Add(new MegaBattleship());
                boats.Add(new MegaCarrier());
            }

            return boats;
        }
    }
}