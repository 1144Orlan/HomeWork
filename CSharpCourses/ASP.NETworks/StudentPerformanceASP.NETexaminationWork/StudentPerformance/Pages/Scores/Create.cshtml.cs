using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentPerformance.Data;
using StudentPerformance.Models;

namespace StudentPerformance.Pages.Scores
{
    public class CreateModel : PageModel
    {
        private readonly StudentPerformance.Data.ApplicationDbContext _context;

        public CreateModel(StudentPerformance.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        //ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name");
            ViewData["StudentId"] = new SelectList(_context.Students.Select(s => new
            {
                s.Id,
                DisplayName = $"{s.Name} ({s.StudentId})"
            }), "Id", "DisplayName");
            return Page();
        }

        [BindProperty]
        public Score Score { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Scores.Add(Score);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
