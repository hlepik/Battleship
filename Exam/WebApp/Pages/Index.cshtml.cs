using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DAL.ApplicationDbContext _context;
        public IList<Quiz>? Quizzes { get;set; }
        [BindProperty(SupportsGet = true)]
        public string? Title { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Btn { get; set; }


        public IndexModel(ILogger<IndexModel> logger, DAL.ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync()
        {

            var query = _context.Quizzes!
                .AsQueryable();


            if (Btn == "Reset")
            {
                Title = "";
                Btn = "";
                return RedirectToPage("/Index");
            }

            Title = Title?.Trim();
            if (!string.IsNullOrWhiteSpace(Title))
            {
                query = query.Where(m => m.Title!.Contains(Title));
            }
            Quizzes = await query.OrderByDescending(x =>x.CreatedAt).ToListAsync();
            return Page();
        }

    }
}
