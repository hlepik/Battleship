using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages.PlayerForm
{
    public class Index : PageModel
    {
        public BattleShip Battleship { get; set; } = new BattleShip();
        [BindProperty,MinLength(2),MaxLength(128)] public string PlayerA { get; set; } = null!;
        [BindProperty,MinLength(2),MaxLength(128)] public string PlayerB { get; set; } = null!;
        [BindProperty]public int Width { get; set; }
        [BindProperty]public int Height { get; set; }
        public string Message { get; set; } = "";
        [BindProperty]public EBoatsCanTouch GameRule { get; set; }
        [BindProperty]public ENextMoveAfterHit NextMove { get; set; }
        [BindProperty(SupportsGet = true)]public string Ai { get; set; } = "";
        public CellState[,] Board1 { get; set; } = null!;
        public CellState[,] Board2 { get; set; } = null!;
        public List<Ship> Boats { get; set; } = null!;


        [BindProperty, MinLength(2), MaxLength(128)] public string GameName { get; set; } = null!;


        private readonly DAL.AppDbContext _context;
        private readonly ILogger<Index> _logger;

        public Index(ILogger<Index> logger, DAL.AppDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task OnGetAsync(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                Ai = "AI";
                PlayerB = "AI";
            }
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {

            if (!string.IsNullOrWhiteSpace(id))
            {
                PlayerB = "AI";
                Ai = "AI";
            }


            var boats = new BoatCount();

            Battleship.Width = Width;
            Battleship.Height = Height;
            Battleship.Player1 = PlayerA;
            Battleship.Player2 = PlayerB;
            Board1 = Battleship.GetBoard(PlayerA);
            Board2 = Battleship.GetBoard(PlayerB);


            if (PlayerA == PlayerB)
            {
                Message = "Player names can't be same!";
                return Page();
            }

            if (PlayerB == "AI" && string.IsNullOrWhiteSpace(GameName) || string.IsNullOrWhiteSpace(PlayerA)|| GameName.Length <2 ||
                GameName.Length > 128 || PlayerA.Length < 2 || PlayerA.Length > 128)
            {
                return Page();
            }

            var gameOption = new GameOption()
            {
                BoardWidth = Width,
                BoardHeight = Height,
                EBoatsCanTouch = GameRule,
                ENextMoveAfterHit = NextMove,
                Name = GameName
            };
            _context.Add(gameOption);

            var playerA = new Player()
            {
                Name = PlayerA,
            };
            var playerB = new Player()
            {
                Name = PlayerB,
            };
            if (!string.IsNullOrWhiteSpace(Ai))
            {
                playerB.EPlayerType = EPlayerType.Ai;
            }

            _context.Players.Add(playerB);
            _context.Players.Add(playerA);
            await _context.SaveChangesAsync();
            Boats = boats.BoatsCount(Battleship.Width, Battleship.Height);
            foreach (var each in Boats)
            {
                var gameBoat = new GameBoat()
                {
                    Name = each.Name,
                    Size = each.Width,
                    LifeCount = each.Width,
                    IsSunken = false,
                    Direction = "",
                    ShipId = 0

                };
                gameBoat.PlayerId = playerA.PlayerId;
                _context.GameBoats!.Add(gameBoat);
            }
            foreach (var each in Boats)
            {
                var gameBoat = new GameBoat()
                {
                    Name = each.Name,
                    Size = each.Width,
                    LifeCount = each.Width,
                    IsSunken = false,
                    Direction = "",
                    ShipId = 0

                };
                gameBoat.PlayerId = playerB.PlayerId;
                _context.GameBoats!.Add(gameBoat);
            }
            var game = new Game()
            {
                Date = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                GameOptionId = gameOption.GameOptionId,
                PlayerAId = playerA.PlayerId,
                PlayerBId = playerB.PlayerId
            };

            var playerABoardState = new PlayerBoardState()
            {
                BoardState = Battleship.GetSerializedGameState(Board1),
                PlayerId = playerA.PlayerId
            };
            var playerBBoardState = new PlayerBoardState()
            {
                BoardState = Battleship.GetSerializedGameState(Board2),
                PlayerId = playerB.PlayerId
            };

            playerABoardState.PlayerId = playerA.PlayerId;
            playerBBoardState.PlayerId = playerB.PlayerId;


            _context.PlayerBoardStates.Add(playerABoardState);
            _context.PlayerBoardStates.Add(playerBBoardState);
            _context.Games!.Add(game);
            await _context.SaveChangesAsync();
            playerA.GameId = game.GameId;
            playerB.GameId = game.GameId;

            await _context.SaveChangesAsync();
            Ai = null!;
            return RedirectToPage("/PlaceTheBoats/Index", new {id = game.GameId});
        }
    }
}