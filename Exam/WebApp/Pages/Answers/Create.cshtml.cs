using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace WebApp.Pages.Answers
{
    public class CreateModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public CreateModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public string Message { get; set; } = "";
        public SelectList SelectList { get; set; } = null!;
        public IActionResult OnGet(int id)
        {
            if (id == null)
            {
                SelectList =
                    new SelectList(
                        _context.Question!.ToList(),
                        nameof(Question.QuestionId),
                        nameof(Question.Questions));
            }
            else
            {
                SelectList = new SelectList(
                        _context.Question!.ToList().Where(p =>p.QuestionId == id),
                        nameof(Question.QuestionId),
                        nameof(Question.Questions));

            }

            return Page();
        }
        [BindProperty]
        public Answer? Answer { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            SelectList =
                new SelectList(
                    _context.Question!.ToList(),
                    nameof(Question.QuestionId),
                    nameof(Question.Questions));

            if (Answer!.IsAnswerCorrect)
            {
                foreach (var each in _context.Question.Where(p => p.QuestionId == Answer.QuestionId))
                {
                    if (each.IsHavingAnswer)
                    {
                        foreach (var all in _context.Answers.Where(p => p.QuestionId == Answer.QuestionId))
                        {
                            if (all.IsAnswerCorrect)
                            {
                                Message = "This question has already a correct answer!";
                                return Page();
                            }
                        }
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Answers.Add(Answer);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
