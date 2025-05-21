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
    public class IndexModel : PageModel
    {
        private readonly FitnessClubAutomation.Data.ApplicationDbContext _context;

        public IndexModel(FitnessClubAutomation.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<TrainingSession> TrainingSession { get;set; } = default!;

        public async Task OnGetAsync()
        {
            TrainingSession = await _context.TrainingSessions
                .Include(t => t.Service).ToListAsync();
        }
    }
}
