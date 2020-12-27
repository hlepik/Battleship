using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages.AnsweringQuiz
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

        [BindProperty(SupportsGet = true)]
        public int UserChoice { get; set; }

        [BindProperty]
        public int Question { get; set; }



        public async Task<IActionResult> OnGetAsync(int id, string? submit, int? count, int? choice)
        {
            Quiz = await _context.Quizzes!
                .Include(p =>p.Questions)
                .ThenInclude(p =>p.Answer)
                .FirstOrDefaultAsync(p => p.QuizId == id);


            if (submit != null)
            {
                if (count != null)
                {
                    Question = count!.Value;
                }

                Question += 1;

                if (Question == Quiz.Questions!.Count)
                {
                    return RedirectToPage("/Statistics/Index", new {id = Quiz.QuizId});
                }
            }
            return Page();

        }
    }
}