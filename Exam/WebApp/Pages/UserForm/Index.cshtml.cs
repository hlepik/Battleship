using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages.UserForm
{
    public class Index : PageModel
    {
        [BindProperty, MaxLength(128)] public string Player { get; set; } = null!;
        private readonly DAL.ApplicationDbContext _context;


        public Index( DAL.ApplicationDbContext context)
        {
            _context = context;

        }

        public Quiz? Quiz { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {

            Quiz = await _context.Quizzes!
                .Include(p => p.Questions)
                .ThenInclude(p => p.Answer)
                .FirstOrDefaultAsync(p => p.QuizId == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id){
            Quiz = await _context.Quizzes!
                .Include(p => p.Questions)
                .ThenInclude(p => p.Answer)
                .FirstOrDefaultAsync(p => p.QuizId == id);
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var player = new Player()
            {
                Name = Player,
                QuizId = id
            };
            _context.Add(player);
            await _context.SaveChangesAsync();

            var playerId = 0;
            foreach (var each in _context.Players.Where(p =>p.QuizId == id))
            {
                playerId = each.PlayerId;
            }
            var playerAnswer = new PlayerAnswer()
            {
                Count = 0,
                CorrectAnswersCount = 0,
                PlayerId = playerId

            };
            _context.Add(playerAnswer);
            await _context.SaveChangesAsync();
            return RedirectToPage("/AnsweringQuiz/Index", new {id = Quiz.QuizId, playerId = playerId});


        }
    }
}