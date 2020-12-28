using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        [BindProperty(SupportsGet = true)]
        public int Question { get; set; }

        public string Message { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(int id, int playerId)
        {
            Quiz = await _context.Quizzes!
                .Include(p => p.Questions)
                .ThenInclude(p => p.Answer)
                .FirstOrDefaultAsync(p => p.QuizId == id);

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(int id, int playerId)
        {
            Quiz = await _context.Quizzes!
                .Include(p =>p.Questions)
                .ThenInclude(p =>p.Answer)
                .FirstOrDefaultAsync(p => p.QuizId == id);


            if (UserChoice == 0)
            {
                return Page();
            }
            var correct = 0;
            foreach (var each in _context.Answers.Where(p => p.AnswerId == UserChoice))
            {
                if (each.IsAnswerCorrect)
                {
                    correct = 1;
                    Message = "Last Answer was correct!";
                }
                else
                {
                    foreach (var all in _context.Question.Where(p => p.QuestionId == each.QuestionId))
                    {
                        if (!all.IsHavingAnswer)
                        {
                            correct = 1;
                        }

                    }
                }
            }

            foreach (var each in _context.PlayerAnswers.Where(p =>p.PlayerId == playerId))
            {
                each.Count += 1;
                Question = each.Count;
                each.CorrectAnswersCount += correct;
            }
            await _context.SaveChangesAsync();



            if (Question >= Quiz.Questions!.Count)
            {
                return RedirectToPage("/Statistics/Index", new {id = Quiz.QuizId, playerId = playerId});
            }

            UserChoice = 0;
            return Page();

        }
    }
}