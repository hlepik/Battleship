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
        public List<Ship> GameShips { get; set; } = new List<Ship>();


        [BindProperty(SupportsGet = true)]public string? Direction { get; set; }
        [BindProperty(SupportsGet = true)]public int CurrentBoatId { get; set; } = 0;
        public int Size { get; set; } = 0!;
        public string Message { get; set; } = "";


        public void GetBoats(int playerId)
        {
            GameShips = new List<Ship>();
            foreach (var each in _context.Boats.Where(p =>p.PlayerId == playerId))
            {
                GameShips.Add(new Ship(each.Name, each.Size, each.BoatId, each.Inserted));
            }
        }


        public async Task<IActionResult> OnGetAsync(int id, int? x, int? y, string? dir, int? ship)
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

            Game = await _context.Games
                .Where(p => p.GameId == id)
                .Include(p =>p.PlayerA.Boats)
                .Include(p=>p.PlayerB.Boats)
                .Include(p=>p.PlayerA.PlayerBoardStates).Take(1)
                .Include(p=>p.PlayerB.PlayerBoardStates).Take(1)
                .Include(p => p.GameOption).Take(1)
                .Include(p => p.PlayerA)
                // .ThenInclude(p =>p.PlayerBoardStates).Take(1)
                .Include(p => p.PlayerB)
                // .ThenInclude(p =>p.PlayerBoardStates).Take(1)
                .FirstOrDefaultAsync();

            var boardState1 = Game.PlayerA.PlayerBoardStates.Select(p => p.BoardState).LastOrDefault();
            var boardState2 = Game.PlayerB.PlayerBoardStates.Select(p => p.BoardState).LastOrDefault();


            BattleShip.Width = Game.GameOption.BoardWidth;
            BattleShip.Height = Game.GameOption.BoardHeight;
            BattleShip.GameRule = Game.GameOption.EBoatsCanTouch;
            BattleShip.Player1 = Game.PlayerA.Name;
            BattleShip.Player2 = Game.PlayerB.Name;


            BattleShip.SetGameStateFromJsonString(boardState1, boardState2);

            var player = BattleShip.Player1;
            var playerId = Game.PlayerA.PlayerId;
            if (!BattleShip.NextMoveByX)
            {
                player = BattleShip.Player2;
                playerId = Game.PlayerB.PlayerId;
            }

            GetBoats(playerId);

            if (x != null && y != null && dir != null && ship != null)
            {

                var shipName = "";
                foreach (var each in GameShips.Where(p =>p.ShipId == ship))
                {
                    Size = each.Width;
                    shipName = each.Name;
                }

                boat.BoatLocationCheck(BattleShip, x.Value, y.Value, Size, Direction!, player);
                if (!BattleShip.CanInsert)
                {
                    Message = "Can't insert your boat here!";
                }
                else
                {

                    BattleShip.InsertBoat(x.Value, y.Value, player, Size, Direction!, shipName);

                    foreach (var each in GameShips.Where(each => each.ShipId == CurrentBoatId))
                    {
                        each.Inserted = true;
                    }
                    var count = 0;
                    foreach (var each in Game.PlayerA.Boats)
                    {

                        if (each.BoatId == CurrentBoatId)
                        {
                            each.Inserted = true;
                        }
                        if (each.Inserted)
                        {
                            count++;
                        }
                    }
                    if (count == GameShips.Count)
                    {
                        BattleShip.NextMoveByX = !BattleShip.NextMoveByX;


                        GetBoats(Game.PlayerBId);

                    }

                    var boardState = new PlayerBoardState()
                    {
                        PlayerId = Game.PlayerAId,
                        BoardState = BattleShip.GetSerializedGameState(BattleShip.Board1)
                    };
                    _context.PlayerBoardStates.Add(boardState);

                    boardState = new PlayerBoardState()
                    {
                        PlayerId = Game.PlayerBId,
                        BoardState = BattleShip.GetSerializedGameState(BattleShip.Board2)
                    };
                    _context.PlayerBoardStates.Add(boardState);


                    if (BattleShip.Player1 == player)
                    {
                        foreach (var each in BattleShip.Player1Ships)
                        {
                            var gameBoat = new GameBoat()
                            {
                                Name = each.Name,
                                Size = each.Width,
                                IsSunken = each.IsSunken,
                                Direction = each.Direction,
                                LifeCount = each.LifeCount,
                                ShipId = each.ShipId
                            };
                            gameBoat.PlayerId = Game.PlayerAId;
                            _context.GameBoats!.Add(gameBoat);
                        }
                    }
                    else
                    {
                        foreach (var each in BattleShip.Player2Ships)
                        {
                            var gameBoat = new GameBoat()
                            {
                                Name = each.Name,
                                Size = each.Width,
                                IsSunken = each.IsSunken,
                                Direction = each.Direction,
                                LifeCount = each.LifeCount,
                                ShipId = each.ShipId
                            };
                            gameBoat.PlayerId = Game.PlayerBId;
                            _context.GameBoats!.Add(gameBoat);
                        }
                    }
                    await _context.SaveChangesAsync();
                    if(count == GameShips.Count && player == Game.PlayerB.Name)
                    {
                        return RedirectToPage("/NextMove/Index", new {id = Game.GameId});
                    }
                    Direction = null;
                }
            }

            return Page();
        }

    }
}