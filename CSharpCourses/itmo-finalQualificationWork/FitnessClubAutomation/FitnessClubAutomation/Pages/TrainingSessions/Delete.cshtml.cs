using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessClubAutomation.Pages.TrainingSessions
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly FitnessClubAutomation.Data.ApplicationDbContext _context;

        public DeleteModel(FitnessClubAutomation.Data.ApplicationDbContext context)
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

            var trainingsession = await _context.TrainingSessions.FirstOrDefaultAsync(m => m.Id == id);

            if (trainingsession == null)
            {
                return NotFound();
            }
            else
            {
                TrainingSession = trainingsession;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingSession = await _context.TrainingSessions
                .Include(t => t.Service)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainingSession != null)
            {                
                var clientServices = await _context.ClientServices
                    .Where(cs => cs.TrainingSessionId == trainingSession.Id)
                    .ToListAsync();

                if (clientServices.Any())
                {
                    _context.ClientServices.RemoveRange(clientServices);
                }

                _context.TrainingSessions.Remove(trainingSession);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}