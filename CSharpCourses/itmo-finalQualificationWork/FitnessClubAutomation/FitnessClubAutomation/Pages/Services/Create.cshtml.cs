using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessClubAutomation.Pages.Services
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly FitnessClubAutomation.Data.ApplicationDbContext _context;

        public CreateModel(FitnessClubAutomation.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "FullName");
            return Page();
        }

        [BindProperty]
        public Service Service { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            // Remove validation errors for navigation properties
            ModelState.Remove("Service.Coach");
            ModelState.Remove("Service.ClientServices");
            ModelState.Remove("Service.TrainingSessions");

            // Debug: Check what's in ModelState
            foreach (var modelState in ModelState)
            {
                var key = modelState.Key;
                var errors = modelState.Value.Errors;
                foreach (var error in errors)
                {
                    // Log or print the error
                    System.Diagnostics.Debug.WriteLine($"Error in {key}: {error.ErrorMessage}");
                }
            }


            if (!ModelState.IsValid)
            {
                ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "FullName");
                return Page();
            }

            _context.Services.Add(Service);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
