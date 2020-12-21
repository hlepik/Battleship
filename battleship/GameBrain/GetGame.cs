
using System.Linq;
using DAL;


namespace GameBrain
{
    public class GetGame
    {
        public static BattleShip GetGameFromDb(int gameId)
        {
            using var db = new AppDbContext();
            var game = new BattleShip();
            game.GameId = gameId;
            var gameOptionId = 0;

            foreach (var games in db.Games!.Where(x => x.GameId == gameId))
            {

                game.PlayerAId = games.PlayerAId;
                game.PlayerBId = games.PlayerBId;
                game.NextMoveByX = games.NextMoveByX;
                gameOptionId = games.GameOptionId;

            }
            foreach (var gameOption in db.GameOptions.Where(x => x.GameOptionId == gameOptionId))
            {
                game.Width = gameOption.BoardWidth;
                game.Height = gameOption.BoardHeight;
                game.GameRule = gameOption.EBoatsCanTouch;
                game.NextMove = gameOption.ENextMoveAfterHit;

            }

            foreach (var players in db.Players.Where(x => x.PlayerId == game.PlayerAId))
            {

                game.Player1 = players.Name;
                game.PlayerType1 = players.EPlayerType;
            }
            foreach (var players in db.Players.Where(x => x.PlayerId == game.PlayerBId))
            {

                game.Player2 = players.Name;
                game.PlayerType2 = players.EPlayerType;
            }


            var boardState1 = db.PlayerBoardStates
                .OrderByDescending(p => p.CreatedAt)
                .Where(p => p.PlayerId == game.PlayerAId)
                .Select(p =>p.BoardState)
                .FirstOrDefault();

            var boardState2 = db.PlayerBoardStates
                .OrderByDescending(p => p.CreatedAt)
                .Where(p => p.PlayerId == game.PlayerBId)
                .Select(p =>p.BoardState)
                .FirstOrDefault();


            foreach (var boat in db.GameBoats!.Where(x => x.PlayerId == game.PlayerAId))
            {

                game.Player1Ships.Add(new Ship(boat.Name,boat.Size,boat.Direction,boat.GameBoatId, boat.IsSunken, boat.LifeCount));
            }
            foreach (var boat in db.GameBoats!.Where(x => x.PlayerId == game.PlayerBId))
            {

                game.Player2Ships.Add(new Ship(boat.Name,boat.Size,boat.Direction,boat.GameBoatId, boat.IsSunken, boat.LifeCount));
            }
            game.SetGameStateFromJsonString(boardState1!, game.Player1);
            game.SetGameStateFromJsonString(boardState2!, game.Player2);

            return game;
        }
    }

}