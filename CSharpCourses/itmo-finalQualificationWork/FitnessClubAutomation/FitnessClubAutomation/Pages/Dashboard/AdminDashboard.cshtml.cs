using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FitnessClubAutomation.Pages.Dashboard
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AdminDashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int TotalClients { get; set; }
        public int PermanentClients { get; set; }
        public int OneTimeClients { get; set; }
        public int TotalServices { get; set; }
        public int GroupServices { get; set; }
        public int IndividualServices { get; set; }

        public async Task OnGetAsync()
        {
            TotalClients = await _context.Clients.CountAsync();
            PermanentClients = await _context.Clients.CountAsync(c => c.Status == ClientStatus.Permanent);
            OneTimeClients = await _context.Clients.CountAsync(c => c.Status == ClientStatus.OneTime);

            TotalServices = await _context.Services.CountAsync();
            GroupServices = await _context.Services.CountAsync(s => s.Type == TrainingType.Group);
            IndividualServices = await _context.Services.CountAsync(s => s.Type == TrainingType.Individual);
        }
    }
}