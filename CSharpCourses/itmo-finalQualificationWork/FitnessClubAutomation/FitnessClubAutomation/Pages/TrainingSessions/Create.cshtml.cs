using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessClubAutomation.Pages.TrainingSessions
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {            
            var services = _context.Services
                .Include(s => s.Coach)
                .Select(s => new
                {
                    Id = s.Id,
                    DisplayName = $"{s.Name} - {s.Type} - Coach: {s.Coach.FullName}"
                })
                .ToList();

            ViewData["ServiceId"] = new SelectList(services, "Id", "DisplayName");

            var today = DateTime.Now.Date;
            var defaultTime = new DateTime(today.Year, today.Month, today.Day, 12, 0, 0);

            TrainingSession = new TrainingSession
            {
                DateTime = defaultTime,
                CurrentParticipants = 0,
                MaxParticipants = 15
            };

            return Page();
        }

        [BindProperty]
        public TrainingSession TrainingSession { get; set; } = default!;

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

                ViewData["ServiceId"] = new SelectList(services, "Id", "DisplayName");
                return Page();
            }

            _context.TrainingSessions.Add(TrainingSession);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
