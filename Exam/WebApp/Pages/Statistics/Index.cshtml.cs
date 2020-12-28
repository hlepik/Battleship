using System.Linq;
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
        public int Count { get; set; }
        public int CorrectAnswers { get; set; }
        public int AllCount { get; set; }
        public int AllAnswers { get; set; }
        public int QuestionsCount { get; set; }
        public double CalculatedResult { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, int playerId, string? submit)
        {


            Quiz = await _context.Quizzes!
                .Include(p => p.Questions)
                .ThenInclude(p => p.Answer)
                .FirstOrDefaultAsync(p => p.QuizId == id);


            foreach (var each in _context.Players.Where(p =>p.QuizId == id))
            {
                AllCount += 1;
                foreach (var all in _context.PlayerAnswers.Where(p =>p.PlayerId == each.PlayerId))
                {
                    AllAnswers += all.CorrectAnswersCount;
                }
            }

            foreach (var each in _context.Question.Where(p =>p.QuizId == id))
            {
                QuestionsCount += 1;
            }

            foreach (var each in _context.PlayerAnswers.Where(p =>p.PlayerId == playerId))
            {
                Count = each.Count;
                CorrectAnswers = each.CorrectAnswersCount;

            }

            if (submit != null)
            {
                return RedirectToPage("/Pages/Index");
            }

            CalculatedResult = AllAnswers * QuestionsCount / AllCount;
            return Page();
        }

    }
}