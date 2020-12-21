using System.Linq;
using DAL;
using Domain;

namespace GameBrain
{
    public class GameUpdate
    {
        public void Update(BattleShip game)
        {

            using var db = new AppDbContext();

            foreach (var each in db.Games!.Where(x =>x.GameId == game.GameId))
            {
                each.NextMoveByX = game.NextMoveByX;
            }

            var boardState = new PlayerBoardState()
            {
                PlayerId = game.PlayerAId,
                BoardState = game.GetSerializedGameState(game.Board1)
            };
            db.PlayerBoardStates.Add(boardState);
            boardState = new PlayerBoardState()
            {
                PlayerId = game.PlayerBId,
                BoardState = game.GetSerializedGameState(game.Board2)
            };
            db.PlayerBoardStates.Add(boardState);


            foreach (var each in db.GameBoats!.Where(p =>p.PlayerId == game.PlayerAId))
            {
                foreach (var gameBoat in game.Player1Ships)
                {
                    if (gameBoat.ShipId == each.ShipId)
                    {
                        each.IsSunken = gameBoat.IsSunken;
                        each.LifeCount = gameBoat.LifeCount;
                    }
                }
            }
            foreach (var each in db.GameBoats!.Where(p =>p.PlayerId == game.PlayerBId))
            {
                foreach (var gameBoat in game.Player2Ships)
                {
                    if (gameBoat.ShipId == each.ShipId)
                    {
                        each.LifeCount = gameBoat.LifeCount;
                    }
                }
            }
            db.SaveChanges();
        }
    }
}