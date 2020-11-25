using System;
using System.Linq;
using DAL;
using Domain;

namespace GameBrain
{
    public class GetGame
    {
        public static BattleShip GetGameFromDb(int gameOptionId)
        {
            using var db = new AppDbContext();
            var ship = new Ship();
            var game = new BattleShip();
            var gameId = 0;
            var player1Id = 0;
            var player2Id = 0;

            foreach (var gameOption in db.GameOptions.Where(x => x.GameOptionId == gameOptionId))
            {
                game.Width = gameOption.BoardWidth;
                game.Height = gameOption.BoardHeight;
                game.GameRule = gameOption.EBoatsCanTouch;
                game.NextMove = gameOption.ENextMoveAfterHit;
            }

            foreach (var games in db.Games.Where(x => x.GameOptionId == gameOptionId))
            {

                player1Id = games.PlayerAId;
                player2Id = games.PlayerBId;
            }
            foreach (var players in db.Players.Where(x => x.PlayerId == player1Id))
            {

                game.Player1 = players.Name;
                game.PlayerType1 = players.EPlayerType;
            }
            foreach (var players in db.Players.Where(x => x.PlayerId == player2Id))
            {

                game.Player2 = players.Name;
                game.PlayerType2 = players.EPlayerType;
            }

            string boardState1 = "";
            string boardState2 = "";
            foreach (var board in db.PlayerBoardStates.Where(x => x.PlayerId == player1Id))
            {

                boardState1 = board.BoardState;
            }

            foreach (var board in db.PlayerBoardStates.Where(x => x.PlayerId == player2Id))
            {

                boardState2 = board.BoardState;
            }


            foreach (var boat in db.GameBoats.Where(x => x.PlayerId == player1Id))
            {

                game.Player1Ships.Add(new Ship(boat.Name,boat.Size,boat.Direction,boat.ShipId, boat.IsSunken));
                ship.LifeCount = boat.LifeCount;
            }
            foreach (var boat in db.GameBoats.Where(x => x.PlayerId == player2Id))
            {

                game.Player2Ships.Add(new Ship(boat.Name,boat.Size,boat.Direction,boat.ShipId, boat.IsSunken));
                ship.LifeCount = boat.LifeCount;
            }
            game.SetGameStateFromJsonString(boardState1, boardState2);

            return game;
        }
    }
}