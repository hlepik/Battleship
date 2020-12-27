using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Answer
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public DeleteModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Answers? Answers { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Answers = await _context.Answer
                .Include(a => a.Question).FirstOrDefaultAsync(m => m.AnswersId == id);

            if (Answers == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Answers = await _context.Answer!.FindAsync(id);

            if (Answers != null)
            {
                _context.Answer.Remove(Answers);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
