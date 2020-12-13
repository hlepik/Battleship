using System;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages.GamePlay
{
    public class Index : PageModel
    {
        private readonly DAL.AppDbContext _context;
        private readonly ILogger<Index> _logger;

        public Index(ILogger<Index> logger, DAL.AppDbContext context)
        {
            _context = context;
            _logger = logger;

        }
        public Game? Game { get; set; }
        public Player? Player1 { get; set; }
        public Player? Player2 { get; set; }
        public BattleShip BattleShip { get; set; } = new BattleShip();

        public string Message { get; set; } = "";

        public void SaveGameToDb(string player)
        {
            if (player == Game!.PlayerA.Name)
            {
                var boardState = new PlayerBoardState()
                {
                    PlayerId = Game!.PlayerBId,
                    BoardState = BattleShip.GetSerializedGameState(BattleShip.Board2)
                };
                _context.PlayerBoardStates.Add(boardState);
                Player playerMove = Player2!;
                if (BattleShip.NextMoveByX)
                {
                    playerMove = Player1!;
                }

                if (!BattleShip.TextWhenMiss)
                {
                    foreach (var each in playerMove.GameBoats.Where(x => x.ShipId == BattleShip.ShipId))
                    {
                        each.IsSunken = BattleShip.Player2Ships.Where(x => x.ShipId == BattleShip.ShipId)
                            .Select(x => x.IsSunken).FirstOrDefault();
                        each.LifeCount = BattleShip.Player2Ships.Where(x => x.ShipId == BattleShip.ShipId)
                            .Select(x => x.LifeCount).FirstOrDefault();
                    }
                }
            }
            else
            {
                var boardState = new PlayerBoardState()
                {
                    PlayerId = Game.PlayerAId,
                    BoardState = BattleShip.GetSerializedGameState(BattleShip.Board1)
                };
                _context.PlayerBoardStates.Add(boardState);
                if (!BattleShip.TextWhenMiss)
                {
                    foreach (var each in Game.PlayerA.GameBoats.Where(x => x.ShipId == BattleShip.ShipId))
                    {
                        each.IsSunken = BattleShip.Player1Ships.Where(x => x.ShipId == BattleShip.ShipId)
                            .Select(x => x.IsSunken).FirstOrDefault();
                        each.LifeCount = BattleShip.Player1Ships.Where(x => x.ShipId == BattleShip.ShipId)
                            .Select(x => x.LifeCount).FirstOrDefault();
                    }
                }
            }
        }



        public async Task<IActionResult> OnGetAsync(int id, int? x, int? y)
        {

            Game = await _context.Games!
                .Where(p => p.GameId == id)
                .Include(p => p.GameOption)
                .Include(p => p.PlayerA)
                .Include(p => p.PlayerB)
                .FirstOrDefaultAsync();

            Player1 = _context.Players
                .Include(p => p.PlayerBoardStates)
                .Include(p=>p.GameBoats)
                .FirstOrDefault(p => p.PlayerId == Game.PlayerAId);
            Player2 = _context.Players
                .Include(p => p.PlayerBoardStates)
                .Include(p=>p.GameBoats)
                .FirstOrDefault(p => p.PlayerId == Game.PlayerBId);

            BattleShip.Width = Game.GameOption.BoardWidth;
            BattleShip.Height = Game.GameOption.BoardHeight;
            BattleShip.Player1 = Game.PlayerA.Name;
            BattleShip.Player2 = Game.PlayerB.Name;
            BattleShip.GameRule = Game.GameOption.EBoatsCanTouch;
            BattleShip.NextMoveByX = Game.NextMoveByX;
            BattleShip.PlayerType2 = Game.PlayerB.EPlayerType;

            foreach (var each in Player1!.GameBoats)
            {
                BattleShip.Player1Ships.Add(new Ship(each.Name, each.Size, each.Direction, each.ShipId, each.IsSunken, each.LifeCount));
            }

            foreach (var each in Player2!.GameBoats)
            {
                BattleShip.Player2Ships.Add(new Ship(each.Name, each.Size, each.Direction, each.ShipId, each.IsSunken, each.LifeCount));

            }

            var boardState2 = Player2!.PlayerBoardStates.Select(p => p.BoardState).LastOrDefault();
            var boardState1 = Player1!.PlayerBoardStates.Select(p => p.BoardState).LastOrDefault();
            BattleShip.SetGameStateFromJsonString(boardState1!, BattleShip.Player1);
            BattleShip.SetGameStateFromJsonString(boardState2!, BattleShip.Player2);
            var player = BattleShip.Player1;
            var playerBoard = BattleShip.Board2;

            if (!BattleShip.NextMoveByX)
            {
                player = BattleShip.Player2;
                playerBoard = BattleShip.Board1;
            }



            if (Game.PlayerB.EPlayerType == EPlayerType.Ai && !BattleShip.NextMoveByX)
            {
                Random rand = new Random();
                x = rand.Next(0, BattleShip.Width);
                y = rand.Next(0, BattleShip.Height);
                BattleShip.MakeAMove(x.Value, y.Value, playerBoard, BattleShip);
                SaveGameToDb(player);

                Game.NextMoveByX = BattleShip.NextMoveByX;
                await _context.SaveChangesAsync();
                return RedirectToPage("/NextMove/Index", new {id = Game.GameId, message = Message});


            }

            if (x != null && y != null)
            {

                BattleShip.MakeAMove(x.Value, y.Value, playerBoard, BattleShip);
                SaveGameToDb(player);


                if (BattleShip.TextWhenMiss)
                {
                    Message = "You missed!";

                    Game.NextMoveByX = BattleShip.NextMoveByX;
                    await _context.SaveChangesAsync();

                    return RedirectToPage("/NextMove/Index", new {id = Game.GameId, message = Message});
                }
                Message = BattleShip.TextWhenHit ? "Hit!" : "Ship has been destroyed!";

            }
            await _context.SaveChangesAsync();
            // if(Message.Length > 1 && Game.GameOption.ENextMoveAfterHit == ENextMoveAfterHit.SamePlayer)
            // {
            //     return Page();
            // }

            return Page();
        }

    }
}