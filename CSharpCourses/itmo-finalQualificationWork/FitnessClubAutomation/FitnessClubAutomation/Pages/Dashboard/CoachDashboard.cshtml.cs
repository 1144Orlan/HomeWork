using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FitnessClubAutomation.Pages.Dashboard
{
    [Authorize(Roles = "Coach")]
    public class CoachDashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CoachDashboardModel(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Models.Staff? Coach { get; set; }
        public List<Service> Services { get; set; } = new List<Service>();
        public List<TrainingSession> TrainingSessions { get; set; } = new List<TrainingSession>();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {                
                Coach = await _context.Staff
                    .FirstOrDefaultAsync(s => s.Email == user.Email);

                if (Coach != null)
                {                    
                    Services = await _context.Services
                        .Where(s => s.StaffId == Coach.Id)
                        .ToListAsync();
                    
                    TrainingSessions = await _context.TrainingSessions
                        .Include(t => t.Service)
                        .Where(t => t.Service.StaffId == Coach.Id && t.DateTime >= DateTime.Now)
                        .OrderBy(t => t.DateTime)
                        .ToListAsync();
                }
            }
        }
    }
}