using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp;
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
        public Player? Player1 { get; set; }
        public Player? Player2 { get; set; }
        public PlayerBoardState? PlayerBoardStates1 { get; set; }
        public PlayerBoardState? PlayerBoardStates2 { get; set; }
        public BattleShip BattleShip { get; set; } = new BattleShip();
        public List<Ship> GameShips { get; set; } = new List<Ship>();

        [BindProperty(SupportsGet = true)]public string? Direction { get; set; }
        [BindProperty(SupportsGet = true)]public int CurrentBoatId { get; set; } = 0;
        public int Size { get; set; }
        public string Message { get; set; } = "";
        public int? LastShipId { get; set; }
        [BindProperty(SupportsGet = true)]public string? Random { get; set; }
        public string? Ready { get; set; }


        public void GetBoats(int playerId)
        {
            GameShips = new List<Ship>();
            foreach (var each in _context.Boats.Where(p =>p.PlayerId == playerId))
            {
                GameShips.Add(new Ship(each.Name, each.Size, each.BoatId, each.Inserted));
            }
        }

        public void GetRandomBoats(string player)
        {

            var boardState = BattleShip.NextMoveByX
                ? Game!.PlayerA.PlayerBoardStates.Select(p => p.BoardState).FirstOrDefault()
                : Game!.PlayerB.PlayerBoardStates.Select(p => p.BoardState).FirstOrDefault();
            BattleShip.SetGameStateFromJsonString(boardState, player);

            var x = 0;
            var y = 0;
            foreach (var each in GameShips)
            {
                Size = each.Width;
                var shipName = each.Name;
                each.Inserted = true;
                var boat = new CanInsertBoat();

                do
                {
                    Random rand = new Random();
                    x = rand.Next(0, BattleShip.Width);
                    y = rand.Next(0, BattleShip.Height);
                    var num = rand.Next(1, 3);
                    if (num == 1 )
                    {
                        Direction = "R";
                    }
                    else
                    {
                        Direction = "D";
                    }

                    boat.BoatLocationCheck(BattleShip, x, y, Size, Direction!, player);

                }while (!BattleShip.CanInsert);

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
                var boardState = new PlayerBoardState()
                {
                    PlayerId = Game!.PlayerBId,
                    BoardState = BattleShip.GetSerializedGameState(BattleShip.Board2)
                };
                _context.PlayerBoardStates.Add(boardState);
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
                    LastShipId = each.ShipId;
                    gameBoat.PlayerId = Game.PlayerBId;
                    _context.GameBoats!.Add(gameBoat);
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

            Game = await _context.Games
                .Where(p => p.GameId == id)
                .Include(p => p.GameOption)
                .Include(p => p.PlayerA)
                .ThenInclude(p => p.PlayerBoardStates)
                .Include(p => p.PlayerB)
                .ThenInclude(p => p.PlayerBoardStates)
                .FirstOrDefaultAsync();

            BattleShip.Width = Game.GameOption.BoardWidth;
            BattleShip.Height = Game.GameOption.BoardHeight;
            BattleShip.GameRule = Game.GameOption.EBoatsCanTouch;
            BattleShip.Player1 = Game.PlayerA.Name;
            BattleShip.Player2 = Game.PlayerB.Name;
            BattleShip.NextMoveByX = Game.NextMoveByX;

            var player = BattleShip.Player1;
            var playerId = Game.PlayerA.PlayerId;


            if (!BattleShip.NextMoveByX)
            {
                player = BattleShip.Player2;
                playerId = Game.PlayerB.PlayerId;
            }

            var boardState1 = Game.PlayerA.PlayerBoardStates.Select(p => p.BoardState).LastOrDefault();
            var boardState2 = Game.PlayerB.PlayerBoardStates.Select(p => p.BoardState).LastOrDefault();

            BattleShip.SetGameStateFromJsonString(boardState1!, Game.PlayerA.Name);
            BattleShip.SetGameStateFromJsonString(boardState2!, Game.PlayerB.Name);

            GetBoats(playerId);


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
                    foreach (var each in GameShips.Where(p => p.ShipId == ship))
                    {
                        Size = each.Width;
                        shipName = each.Name;
                        BattleShip.ShipId = each.ShipId;
                        each.Inserted = true;
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
                        foreach (var each in Game.PlayerA.Boats.Where(each => each.BoatId == CurrentBoatId))
                        {
                            each.Inserted = true;

                        }

                        SaveToDb(player);

                    }

                }
                if (ready != null)
                {
                    if (Random == null)
                    {
                        foreach (var each in BattleShip.NextMoveByX ? Game.PlayerA.Boats:Game.PlayerB.Boats)
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
                    }

                    if (Random != null || count == Game.PlayerA.Boats.Count)
                    {
                        BattleShip.NextMoveByX = !BattleShip.NextMoveByX;
                        Random = null;
                        Game.NextMoveByX = BattleShip.NextMoveByX;
                    }
                    GetBoats(Game.PlayerBId);
                }



                if (!BattleShip.NextMoveByX && Game.PlayerB.EPlayerType == EPlayerType.Ai)
                {
                    GetRandomBoats(Game.PlayerB.Name);
                    SaveToDb(Game.PlayerB.Name);
                    BattleShip.NextMoveByX = !BattleShip.NextMoveByX;
                    Game.NextMoveByX = BattleShip.NextMoveByX;
                    await _context.SaveChangesAsync();
                    return RedirectToPage("/NextMove/Index", new {id = Game.GameId});
                }

                if(count == GameShips.Count || player == Game.PlayerB.Name && ready != null)
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