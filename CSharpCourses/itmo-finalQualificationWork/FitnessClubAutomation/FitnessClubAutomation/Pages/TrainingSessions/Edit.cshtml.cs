using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessClubAutomation.Pages.TrainingSessions
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
        public TrainingSession TrainingSession { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingSession = await _context.TrainingSessions
                .Include(t => t.Service)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainingSession == null)
            {
                return NotFound();
            }

            TrainingSession = trainingSession;
                        
            var dateTime = TrainingSession.DateTime;
            TrainingSession.DateTime = new DateTime(
                dateTime.Year, dateTime.Month, dateTime.Day,
                dateTime.Hour, dateTime.Minute, 0);

            var services = _context.Services
                .Include(s => s.Coach)
                .Select(s => new
                {
                    Id = s.Id,
                    DisplayName = $"{s.Name} - {s.Type} - Coach: {s.Coach.FullName}"
                })
                .ToList();

            ViewData["ServiceId"] = new SelectList(services, "Id", "DisplayName", TrainingSession.ServiceId);

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {            
            ModelState.Remove("TrainingSession.Service");

            if (!ModelState.IsValid)
            {                
                var services = _context.Services
                    .Include(s => s.Coach)
                    .Select(s => new
                    {
                        Id = s.Id,
                        DisplayName = $"{s.Name} - {s.Type} - Coach: {s.Coach.FullName}"
                    })
                    .ToList();

                ViewData["ServiceId"] = new SelectList(services, "Id", "DisplayName", TrainingSession.ServiceId);
                return Page();
            }

            try
            {
                _context.Attach(TrainingSession).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingSessionExists(TrainingSession.Id))
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

        private bool TrainingSessionExists(int id)
        {
            return _context.TrainingSessions.Any(e => e.Id == id);
        }
    }
}