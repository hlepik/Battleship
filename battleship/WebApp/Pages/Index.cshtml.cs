using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DAL.AppDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, DAL.AppDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Game>? Game { get;set; }
        [BindProperty(SupportsGet = true)]
        public string? FileName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Btn { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            Game = await _context.Games!
                .Include(g => g.GameOption)
                .Include(g => g.PlayerA)
                .Include(g => g.PlayerB)
                .OrderByDescending(g => g.Date).ToListAsync();

            var query = _context.Games!
                .Include(r => r.GameOption).AsQueryable();

            if (Btn == "Reset")
            {
                FileName = "";
                Btn = "";
                return RedirectToPage("/Index");
            }

            FileName = FileName?.Trim();
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                query = query.Where(m => m.GameOption.Name.Contains(FileName));
            }
            Game = await query.OrderByDescending(x =>x.Date).ToListAsync();
            return Page();
        }


        public IActionResult OnPostAsync(string? ai)
        {
            return ai != null ? RedirectToPage("./PlayerForm/Index", new {id = ai}) : RedirectToPage("./PlayerForm/Index");
        }
    }
}