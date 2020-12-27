using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages.Statistics
{
    public class Index : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;
        private readonly ILogger<Index> _logger;

        public Index(ILogger<Index> logger, DAL.ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }
        public Quiz? Quiz { get; set; }
        public PlayerAnswer? PlayerAnswer { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, string? submit)
        {

            Quiz = await _context.Quizzes!
                .Include(p => p.Questions)
                .ThenInclude(p => p.Answer)
                .FirstOrDefaultAsync(p => p.QuizId == id);


            if (submit != null)
            {
                return RedirectToPage("/Pages/Index");
            }

            return Page();
        }

    }
}