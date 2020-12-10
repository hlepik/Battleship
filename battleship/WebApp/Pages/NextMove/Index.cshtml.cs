using System.Linq;
using System.Threading.Tasks;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages.NextMove
{
    public class Index : PageModel
    {
        public BattleShip BattleShip { get; set; } = new BattleShip();
        public Game? Game { get; set; }
        public string Message { get; set; } = "";

        private readonly DAL.AppDbContext _context;
        private readonly ILogger<Index> _logger;

        public Index(ILogger<Index> logger, DAL.AppDbContext context)
        {
            _context = context;
            _logger = logger;

        }
        public async Task<IActionResult> OnGetAsync(int id, string? submit, string message)
        {

            Game = await _context.Games
                .Where(x => x.GameId == id)
                .Include(x => x.PlayerA)
                .ThenInclude(x =>x.PlayerBoardStates).Take(1)
                .Include(x => x.PlayerB)
                .ThenInclude(x =>x.PlayerBoardStates).Take(1)
                .FirstOrDefaultAsync();


            var boardState1 = Game.PlayerA.PlayerBoardStates.Select(x => x.BoardState).Last();
            var boardState2 = Game.PlayerB.PlayerBoardStates.Select(x => x.BoardState).Last();

            BattleShip.SetGameStateFromJsonString(boardState1, boardState2);
            BattleShip.Player1 = Game.PlayerA.Name;
            BattleShip.Player2 = Game.PlayerB.Name;

            if (submit != null)
            {
                return RedirectToPage("/GamePlay/Index", new {id = Game.GameId});
            }

            return Page();

        }
    }
}