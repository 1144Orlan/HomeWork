using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FitnessClubAutomation.Pages.Employees
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Staff Staff { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff.FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }
            Staff = staff;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {            
            ModelState.Remove("Staff.Services");
                        
            foreach (var modelState in ModelState)
            {
                var key = modelState.Key;
                var errors = modelState.Value.Errors;
                foreach (var error in errors)
                {                    
                    System.Diagnostics.Debug.WriteLine($"Error in {key}: {error.ErrorMessage}");
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _context.Attach(Staff).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffExists(Staff.Id))
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

        private bool StaffExists(int id)
        {
            return _context.Staff.Any(e => e.Id == id);
        }
    }
}