using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;

namespace WebApp.Pages.Questions
{
    public class CreateModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public CreateModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        // public IActionResult OnGet()
        // {
        // ViewData["QuizId"] = new SelectList(_context.Quizzes, "QuizId", "QuizId");
        //     return Page();
        // }
        public SelectList TitleSelectList { get; set; } = null!;
        public IActionResult OnGet()
        {
            TitleSelectList =
                new SelectList(
                    _context.Quizzes.ToList(),
                    nameof(Quiz.QuizId),
                    nameof(Quiz.Title));
            return Page();
        }


        [BindProperty]
        public Question? Question { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Question!.Add(Question);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
