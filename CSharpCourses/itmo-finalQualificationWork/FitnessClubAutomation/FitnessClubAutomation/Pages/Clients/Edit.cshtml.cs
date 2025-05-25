using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FitnessClubAutomation.Pages.Clients
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
        public Client Client { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            Client = client;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {            
            ModelState.Remove("Client.ClientServices");

            // Debug info
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
                _context.Attach(Client).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(Client.Id))
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

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}