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

namespace WebApp.Pages.Questions
{
    public class EditModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public EditModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Question? Question { get; set; }

        public SelectList TitleSelectList { get; set; } = null!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            TitleSelectList =
                new SelectList(
                    _context.Quizzes.ToList(),
                    nameof(Quiz.QuizId),
                    nameof(Quiz.Title));

            if (id == null)
            {
                return NotFound();
            }

            Question = await _context.Question
                .Include(q => q.Quiz).FirstOrDefaultAsync(m => m.QuestionId == id);

            if (Question == null)
            {
                return NotFound();
            }
            ViewData["QuizId"] = new SelectList(_context.Quizzes, "QuizId", "QuizId");
            return Page();


        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            TitleSelectList =
                new SelectList(
                    _context.Quizzes.ToList(),
                    nameof(Quiz.QuizId),
                    nameof(Quiz.Title));

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(Question!.QuestionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.QuestionId == id);
        }
    }
}
