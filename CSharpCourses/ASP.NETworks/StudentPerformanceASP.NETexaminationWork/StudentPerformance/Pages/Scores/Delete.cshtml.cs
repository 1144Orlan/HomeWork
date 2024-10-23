using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentPerformance.Data;
using StudentPerformance.Models;

namespace StudentPerformance.Pages.Scores
{
    public class DeleteModel : PageModel
    {
        private readonly StudentPerformance.Data.ApplicationDbContext _context;

        public DeleteModel(StudentPerformance.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Score Score { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var score = await _context.Scores.FirstOrDefaultAsync(m => m.Id == id);

            if (score == null)
            {
                return NotFound();
            }
            else
            {
                Score = score;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var score = await _context.Scores.FindAsync(id);
            if (score != null)
            {
                Score = score;
                _context.Scores.Remove(Score);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
