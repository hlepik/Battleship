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

namespace WebApp.Pages.PlayerAnswers
{
    public class EditModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public EditModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PlayerAnswer? PlayerAnswer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PlayerAnswer = await _context.PlayerAnswers
                .Include(p => p.Player).FirstOrDefaultAsync(m => m.PlayerAnswerId == id);

            if (PlayerAnswer == null)
            {
                return NotFound();
            }
           ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "PlayerId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PlayerAnswer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerAnswerExists(PlayerAnswer!.PlayerAnswerId))
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

        private bool PlayerAnswerExists(int id)
        {
            return _context.PlayerAnswers.Any(e => e.PlayerAnswerId == id);
        }
    }
}
