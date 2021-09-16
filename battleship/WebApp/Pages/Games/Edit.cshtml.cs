using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Games
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Game? Game { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Game = await _context.Games!
                .Include(g => g.GameOption)
                .Include(g => g.PlayerA)
                .ThenInclude(g =>g.GameBoats)
                .Include(g => g.PlayerB)
                .ThenInclude(g =>g.GameBoats)
                .FirstOrDefaultAsync(m => m.GameId == id);

            if (Game.PlayerA.GameBoats != null && Game.PlayerB.GameBoats != null)
            {
                if (Game.PlayerB.GameBoats.All(each => each.IsSunken) ||
                    Game.PlayerA.GameBoats.All(each => each.IsSunken))
                {
                    return RedirectToPage("/ViewGame/Index", new {id = Game.GameId});
                }
            }

            if (Game.PlayerB.GameBoats!.Any(each => !each.IsInserted))
            {
                return RedirectToPage("/PlaceTheBoats/Index", new {id = Game.GameId});
            }

            return RedirectToPage("/NextMove/Index", new {id = Game.GameId});
        }
    }
}
