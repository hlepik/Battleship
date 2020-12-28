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

namespace WebApp.Pages.Answers
{
    public class EditModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public EditModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }
        public SelectList SelectList { get; set; } = null!;

        [BindProperty]
        public Answer? Answer { get; set; }
        public string Message { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Answer = await _context.Answers
                .Include(a => a.Question).FirstOrDefaultAsync(m => m.AnswerId == id);

            if (Answer == null)
            {
                return NotFound();
            }
            SelectList =
                new SelectList(
                    _context.Question!.ToList(),
                    nameof(Question.QuestionId),
                    nameof(Question.Questions));
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            SelectList =
                new SelectList(
                    _context.Question!.ToList(),
                    nameof(Question.QuestionId),
                    nameof(Question.Questions));

            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Answer!.IsAnswerCorrect)
            {
                foreach (var each in _context.Question.Where(p =>p.QuestionId == Answer.QuestionId))
                {
                    if (each.IsHavingAnswer)
                    {
                        Message = "This question has already a correct answer!";
                        return Page();
                    }
                }
            }

            _context.Attach(Answer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(Answer!.AnswerId))
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

        private bool AnswerExists(int id)
        {
            return _context.Answers.Any(e => e.AnswerId == id);
        }
    }
}
