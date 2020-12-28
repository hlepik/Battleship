using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Answers
{
    public class IndexModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public IndexModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Answer> Answer { get; set; } = null!;

        public async Task OnGetAsync()
        {
            Answer = await _context.Answers
                .Include(a => a.Question).ToListAsync();
        }
    }
}
