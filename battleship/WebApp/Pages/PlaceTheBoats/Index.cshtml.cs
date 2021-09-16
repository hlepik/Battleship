using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages.PlaceTheBoats
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

        [BindProperty(SupportsGet = true)]public string? Direction { get; set; }
        [BindProperty(SupportsGet = true)]public int CurrentBoatId { get; set; } = 0;
        public int Size { get; set; }
        public string Message { get; set; } = "";
        [BindProperty(SupportsGet = true)]public string? Random { get; set; }
        public string? Ready { get; set; }

        public void GetRandomBoats(string player)
        {

            var boardState1 = _context.PlayerBoardStates
                .OrderBy(p => p.CreatedAt)
                .Where(p => p.PlayerId == Game!.PlayerAId)
                .Select(p =>p.BoardState)
                .FirstOrDefault();

            var boardState2 = _context.PlayerBoardStates
                .OrderBy(p => p.CreatedAt)
                .Where(p => p.PlayerId == Game!.PlayerBId)
                .Select(p =>p.BoardState)
                .FirstOrDefault();


            var boardState = BattleShip.NextMoveByX
                ? boardState1 : boardState2;
            BattleShip.SetGameStateFromJsonString(boardState!, player);


            var y = 0;
            var x = 0;
            foreach (var each in Game!.NextMoveByX ? Game.PlayerA.GameBoats : Game.PlayerB.GameBoats )
            {
                BattleShip.ShipId = each.GameBoatId;
                Size = each.Size;
                var shipName = each.Name;

                each.IsInserted = true;

                var random = new RandomBoats();
                (x, y, Direction) = random.RandomBoat(BattleShip, player, Size);

                BattleShip.InsertBoat(x, y, player, Size, Direction!, shipName);
            }

        }

        public void SaveToDb(string player)
        {
            if (player == BattleShip.Player1)
            {
                var boardState = new PlayerBoardState()
                {
                    PlayerId = Game!.PlayerAId,
                    BoardState = BattleShip.GetSerializedGameState(BattleShip.Board1)
                };
                _context.PlayerBoardStates.Add(boardState);

                foreach (var each in BattleShip.Player1Ships)
                {
                    foreach (var boat in Game.PlayerA.GameBoats.Where(p =>p.GameBoatId == each.ShipId))
                    {
                        boat.Direction = each.Direction;
                        boat.ShipId = each.ShipId;
                    }
                }
            }
            else
            {
                var boardState = new PlayerBoardState()
                {
                    PlayerId = Game!.PlayerBId,
                    BoardState = BattleShip.GetSerializedGameState(BattleShip.Board2)
                };
                _context.PlayerBoardStates.Add(boardState);
                foreach (var each in BattleShip.Player2Ships)
                {
                    foreach (var boat in Game.PlayerB.GameBoats.Where(p =>p.GameBoatId == each.ShipId))
                    {
                        boat.Direction = each.Direction;
                        boat.ShipId = each.ShipId;
                    }
                }
            }
        }

        public async Task<IActionResult> OnGetAsync(int id, int? x, int? y, string? dir, int? ship, string? random, string? ready)
        {

            if (dir != null)
            {
                Direction = dir;
            }

            if (ship != null)
            {
                CurrentBoatId = ship.Value;
            }
            var boat = new CanInsertBoat();

            Game = await _context.Games!
                .Where(p => p.GameId == id)
                .Include(p => p.GameOption)
                .Include(p => p.PlayerA)
                .ThenInclude(p => p.GameBoats)
                .Include(p => p.PlayerB)
                .ThenInclude(p => p.GameBoats)
                .FirstOrDefaultAsync();

            BattleShip.Width = Game.GameOption.BoardWidth;
            BattleShip.Height = Game.GameOption.BoardHeight;
            BattleShip.GameRule = Game.GameOption.EBoatsCanTouch;
            BattleShip.Player1 = Game.PlayerA.Name;
            BattleShip.Player2 = Game.PlayerB.Name;
            BattleShip.NextMoveByX = Game.NextMoveByX;

            var board1 = await _context.PlayerBoardStates
                .OrderBy(p => p.CreatedAt)
                .LastOrDefaultAsync(p => p.PlayerId == Game.PlayerAId);

            var board2 = await _context.PlayerBoardStates
                .OrderBy(p => p.CreatedAt)
                .LastOrDefaultAsync(p => p.PlayerId == Game.PlayerBId);

            var player = BattleShip.Player1;

            if (!BattleShip.NextMoveByX)
            {
                player = BattleShip.Player2;
            }

            BattleShip.SetGameStateFromJsonString(board1.BoardState, BattleShip.Player1);
            BattleShip.SetGameStateFromJsonString(board2.BoardState, BattleShip.Player2);


            if (x != null && y != null && dir != null && ship != null || random != null || ready != null)
            {
                var count = 0;
                var shipName = "";

                if (random != null && ready == null)
                {
                    GetRandomBoats(player);

                    SaveToDb(player);
                }
                else if(x != null && y != null && dir != null && ship != null)
                {
                    foreach (var each in Game.NextMoveByX ? Game.PlayerA.GameBoats : Game.PlayerB.GameBoats)
                    {
                        if (each.GameBoatId == ship)
                        {
                            Size = each.Size;
                            shipName = each.Name;
                            BattleShip.ShipId = each.GameBoatId;
                        }
                    }

                    boat.BoatLocationCheck(BattleShip, x.Value, y.Value, Size, Direction!, player);
                    if (!BattleShip.CanInsert)
                    {
                        Message = "Can't insert your boat here!";
                    }
                    else
                    {

                        BattleShip.InsertBoat(x.Value, y.Value, player, Size, Direction!, shipName);

                        foreach (var each in Game.NextMoveByX ? Game.PlayerA.GameBoats : Game.PlayerB.GameBoats)
                        {
                            if (each.GameBoatId== CurrentBoatId)
                            {
                                each.IsInserted = true;
                            }
                        }

                        SaveToDb(player);
                        await _context.SaveChangesAsync();
                    }

                }
                foreach (var each in BattleShip.NextMoveByX ? Game.PlayerA.GameBoats : Game.PlayerB.GameBoats)
                {
                    if (each.IsInserted)
                    {
                        count++;
                    }
                }

                if (ready != null)
                {
                    if (Random != null || count == Game.PlayerA.GameBoats.Count)
                    {
                        BattleShip.NextMoveByX = !BattleShip.NextMoveByX;
                        Random = null;
                        Game.NextMoveByX = BattleShip.NextMoveByX;
                    }
                }

                Random = null!;
                if (!BattleShip.NextMoveByX && Game.PlayerB.EPlayerType == EPlayerType.Ai)
                {
                    GetRandomBoats(Game.PlayerB.Name);
                    SaveToDb(Game.PlayerB.Name);
                    BattleShip.NextMoveByX = !BattleShip.NextMoveByX;
                    Game.NextMoveByX = BattleShip.NextMoveByX;
                    await _context.SaveChangesAsync();
                    return RedirectToPage("/NextMove/Index", new {id = Game.GameId});
                }

                if(count == Game.PlayerA.GameBoats.Count && player == Game.PlayerB.Name && ready != null)
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("/NextMove/Index", new {id = Game.GameId});
                }
                Ready = null;
            }
            await _context.SaveChangesAsync();
            return Page();
        }

    }
}