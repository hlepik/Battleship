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

namespace WebApp.Pages.Quizzes
{
    public class DetailsModel : PageModel
    {


        public Quiz? Quiz { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            if (Quiz == null)
            {
                return NotFound();
            }



            return Page();
        }
    }
}
