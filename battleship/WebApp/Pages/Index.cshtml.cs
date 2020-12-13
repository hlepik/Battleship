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


        public async Task OnGetAsync()
        {
            Game = await _context.Games!
                .Include(g => g.GameOption)
                .Include(g => g.PlayerA)
                .Include(g => g.PlayerB)
                .OrderBy(g => g.Date).ToListAsync();
        }

        public IActionResult OnPostAsync(string? ai)
        {
            return ai != null ? RedirectToPage("./PlayerForm/Index", new {id = ai}) : RedirectToPage("./PlayerForm/Index");
        }

    }


}