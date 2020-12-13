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

            Game = await _context.Games!
                .Include(p => p.PlayerA)
                .Include(p => p.PlayerB)
                .FirstOrDefaultAsync(p => p.GameId == id);

            if (submit != null)
            {
                return RedirectToPage("/GamePlay/Index", new {id = Game.GameId});
            }

            return Page();

        }

    }
}