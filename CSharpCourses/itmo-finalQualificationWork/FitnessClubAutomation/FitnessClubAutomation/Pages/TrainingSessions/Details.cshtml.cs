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
    public class DetailsModel : PageModel
    {
        private readonly FitnessClubAutomation.Data.ApplicationDbContext _context;

        public DetailsModel(FitnessClubAutomation.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
