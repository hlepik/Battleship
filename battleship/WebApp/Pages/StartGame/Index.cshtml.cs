using System.Data.Entity;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using DAL;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages.StartGame
{
    public class Index : PageModel
    {

        public Game? Game { get; set; }
        public BattleShip BattleShip = new BattleShip();
        public static CellState[,]? Board1 { get; set; }
        public CellState[,]? Board2 { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string PlayerA { get; set; } = null!;
        public string PlayerB { get; set; } = null!;

        private readonly DAL.AppDbContext _context;
        private readonly ILogger<Index> _logger;

        public Index(ILogger<Index> logger, DAL.AppDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        // public async Task OnGetAsync(int id)
        // {
            // //get data from database
            // Game = await _context.Games!.Where(x => x.GameId == id)
            //     .Include(x => x.PlayerA).FirstOrDefaultAsync();
        // }

        // public async Task<IActionResult> OnGetAsync(int? id)
        // {
        //
        //     foreach (var gameOption in _context.GameOptions.Where(x => x.GameOptionId == id))
        //     {
        //         Width = gameOption.BoardWidth;
        //         Height = gameOption.BoardHeight;
        //     }
        //
        //     GetBoard(PlayerA);
        //
        //     return Page();
        // }
        // public CellState[,] GetBoard(string player)
        // {
        //     if (Board1 == null)
        //     {
        //         Board1 = new CellState[Width, Height];
        //         Board2 = new CellState[Width, Height];
        //
        //         for (var row = 0; row < Width; row++)
        //         {
        //             for (var col = 0; col < Height; col++)
        //             {
        //                 Board1[row, col] = new CellState();
        //                 Board2[row, col] = new CellState();
        //             }
        //         }
        //     }
        //
        //     if (player == PlayerA)
        //     {
        //         return Board1;
        //     }
        //
        //     return Board2!;
        //
        // }

    }
}