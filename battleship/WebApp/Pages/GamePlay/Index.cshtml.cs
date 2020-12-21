using System;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;
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
        public BattleShip BattleShip { get; set; } = new BattleShip();

        public string Message { get; set; } = "";
        public string GameOver { get; set; } = "";

        public void SaveGameToDb(string player)
        {


            var boardState = new PlayerBoardState()
            {
                PlayerId = Game!.PlayerBId,
                BoardState = BattleShip.GetSerializedGameState(BattleShip.Board2)
            };
            _context.PlayerBoardStates.Add(boardState);

            var boardState2 = new PlayerBoardState()
            {
                PlayerId = Game.PlayerAId,
                BoardState = BattleShip.GetSerializedGameState(BattleShip.Board1)
            };
            _context.PlayerBoardStates.Add(boardState2);

            if (player == Game!.PlayerA.Name)
            {
                if (!BattleShip.TextWhenMiss)
                {
                    foreach (var each in Game.PlayerB.GameBoats.Where(x => x.ShipId == BattleShip.ShipId))
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
                .Include(p => p.GameOption)
                .Include(p => p.PlayerA)
                .Include(p => p.PlayerA)
                .ThenInclude(p =>p.GameBoats)
                .Include(p => p.PlayerB)
                .Include(p => p.PlayerB)
                .ThenInclude(p =>p.GameBoats)
                .FirstOrDefaultAsync(p => p.GameId == id);



            BattleShip.Width = Game.GameOption.BoardWidth;
            BattleShip.Height = Game.GameOption.BoardHeight;
            BattleShip.Player1 = Game.PlayerA.Name;
            BattleShip.Player2 = Game.PlayerB.Name;
            BattleShip.GameRule = Game.GameOption.EBoatsCanTouch;
            BattleShip.NextMoveByX = Game.NextMoveByX;
            BattleShip.PlayerType2 = Game.PlayerB.EPlayerType;
            BattleShip.NextMove = Game.GameOption.ENextMoveAfterHit;

            foreach (var each in Game.PlayerA.GameBoats)
            {
                BattleShip.Player1Ships.Add(new Ship(each.Name, each.Size, each.Direction, each.ShipId, each.IsSunken, each.LifeCount));
            }

            foreach (var each in Game.PlayerB.GameBoats)
            {
                BattleShip.Player2Ships.Add(new Ship(each.Name, each.Size, each.Direction, each.ShipId, each.IsSunken, each.LifeCount));

            }

            var board1 = await _context.PlayerBoardStates
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync(p => p.PlayerId == Game.PlayerAId);

            var board2 = await _context.PlayerBoardStates
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync(p => p.PlayerId == Game.PlayerBId);

            BattleShip.SetGameStateFromJsonString(board1.BoardState, BattleShip.Player1);
            BattleShip.SetGameStateFromJsonString(board2.BoardState!, BattleShip.Player2);
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
                Game.NextMoveByX = BattleShip.NextMoveByX;
                await _context.SaveChangesAsync();

                if (BattleShip.TextWhenMiss)
                {
                    Message = "You missed!";

                    return RedirectToPage("/NextMove/Index", new {id = Game.GameId, message = Message});
                }
                Message = BattleShip.TextWhenHit ? "Hit!" : "Ship has been destroyed!";

                if (BattleShip.GameIsOver)
                {
                    if (BattleShip.NextMove == ENextMoveAfterHit.OtherPlayer)
                    {
                        BattleShip.NextMoveByX = !BattleShip.NextMoveByX;
                    }
                    GameOver = "Game over!";


                }
                if(!BattleShip.GameIsOver && Game.GameOption.ENextMoveAfterHit == ENextMoveAfterHit.OtherPlayer)
                {
                    return RedirectToPage("/NextMove/Index", new {id = Game.GameId, message = Message});

                }

            }
            BattleShip.GameIsOver = false;

            return Page();
        }

    }
}