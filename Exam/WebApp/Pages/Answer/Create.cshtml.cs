using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;

namespace WebApp.Pages.Answer
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
        // ViewData["QuestionId"] = new SelectList(_context.Question, "QuestionId", "QuestionId");
        //     return Page();
        // }
        public SelectList SelectList { get; set; } = null!;
        public IActionResult OnGet()
        {
            SelectList =
                new SelectList(
                    _context.Question!.ToList(),
                    nameof(Question.QuestionId),
                    nameof(Question.Questions));
            return Page();
        }

        [BindProperty]
        public Answers? Answers { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Answer!.Add(Answers);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
