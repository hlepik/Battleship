using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Games
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]public Game? Game { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Game = await _context.Games!
                .Include(g => g.GameOption)
                .Include(g => g.PlayerA)
                .Include(g => g.PlayerB)
                .FirstOrDefaultAsync(m => m.GameId == id);


            if (Game == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Game = await _context.Games!.FindAsync(id);

            if (Game != null)
            {
                Game = _context.Games
                    .Include(p => p.PlayerA)
                    .Include(p => p.PlayerB)
                    .Include(p => p.GameOption)
                    .FirstOrDefault(p => p.GameId == id);
                _context.Games.Remove(Game!);
                await _context.SaveChangesAsync();

                var boat = _context.GameBoats!
                    .FirstOrDefault(p => p.PlayerId == Game!.PlayerAId);

                if (boat != null)
                {
                    foreach (var each in _context.GameBoats!.Where(p =>p.PlayerId == Game!.PlayerAId))
                    {
                        _context.GameBoats!.Remove(each);
                    }

                }
                boat = _context.GameBoats!
                    .FirstOrDefault(p => p.PlayerId == Game!.PlayerAId);
                if (boat != null)
                {
                    foreach (var each in _context.GameBoats!.Where(p =>p.PlayerId == Game!.PlayerBId))
                    {
                        _context.GameBoats!.Remove(each);
                    }

                }

                foreach (var each in _context.PlayerBoardStates.Where(p =>p.PlayerId == Game!.PlayerAId))
                {
                    _context.PlayerBoardStates!.Remove(each);
                }
                foreach (var each in _context.PlayerBoardStates.Where(p =>p.PlayerId == Game!.PlayerBId))
                {
                    _context.PlayerBoardStates!.Remove(each);
                }

                var gameOption = _context.GameOptions.FirstOrDefault(p => p.GameOptionId == id);
                _context.Remove(gameOption);
                var player1 = _context.Players.FirstOrDefault(p => p.PlayerId == Game!.PlayerAId);
                _context.Remove(player1);
                var player2 = _context.Players.FirstOrDefault(p => p.PlayerId == Game!.PlayerBId);

                _context.Remove(player2);

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Index");
        }
    }
}
