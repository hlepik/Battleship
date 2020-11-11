using System.Linq;
using DAL;

namespace GameBrain
{
    public class GetGame
    {
        public static string GetGameFromDb()
        {

            using (var db = new AppDbContext())
            {
                var game = new BattleShip();
                game.Width = db.GameOptions.Where(x => x.Name == game.FileName).Select(x => x.BoardWidth)
                    .First();
                game.Height = db.GameOptions.Where(x => x.Name == game.FileName).Select(x => x.BoardHeight)
                    .First();
                game.GameRule = db.GameOptions.Where(x => x.Name == game.FileName).Select(x => x.EBoatsCanTouch)
                    .First();
                game.NextMove = db.GameOptions.Where(x => x.Name == game.FileName).Select(x => x.ENextMoveAfterHit)
                    .First();
                var gameOptionId = db.GameOptions.Where(x => x.Name == game.FileName).Select(x => x.GameOptionId)
                    .First();
                var gameId = db.Games.Where(x => x.GameOptionId == gameOptionId).Select(x => x.GameId)
                    .First();
                var player1Id =  db.Games.Where(x => x.GameId == gameId).Select(x => x.PlayerAId)
                    .First();
                var player2Id =  db.Games.Where(x => x.GameId == gameId).Select(x => x.PlayerBId)
                    .First();
                game.Player1 =  db.Players.Where(x => x.PlayerId == player1Id).Select(x => x.Name)
                    .First();
                game.Player2 =  db.Players.Where(x => x.PlayerId == player2Id).Select(x => x.Name)
                    .First();
                game.PlayerType1 =  db.Players.Where(x => x.PlayerId == player1Id).Select(x => x.EPlayerType).First();
                game.PlayerType2 = db.Players.Where(x => x.PlayerId == player2Id).Select(x => x.EPlayerType).First();

                var boardState1 = db.PlayerBoardStates.Where(x => x.PlayerId == player1Id).Select(x => x.BoardState);
                var boardState2 = db.PlayerBoardStates.Where(x => x.PlayerId == player2Id).Select(x => x.BoardState);

                var list1boat = db.GameBoats.Where(x => x.PlayerId == player1Id);








            }

            return "";
        }
    }
}