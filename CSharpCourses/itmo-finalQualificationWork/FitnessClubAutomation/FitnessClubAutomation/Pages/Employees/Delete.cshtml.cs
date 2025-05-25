using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessClubAutomation.Pages.Employees
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DeleteModel(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Staff Staff { get; set; } = default!;

        public int ServicesCount { get; set; }
        public int TrainingSessionsCount { get; set; }
        public int ClientRegistrationsCount { get; set; }

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
            else
            {
                Staff = staff;
                
                ServicesCount = await _context.Services.CountAsync(s => s.StaffId == id);

                var serviceIds = await _context.Services
                    .Where(s => s.StaffId == id)
                    .Select(s => s.Id)
                    .ToListAsync();

                TrainingSessionsCount = await _context.TrainingSessions
                    .CountAsync(t => serviceIds.Contains(t.ServiceId));

                ClientRegistrationsCount = await _context.ClientServices
                    .CountAsync(cs => serviceIds.Contains(cs.ServiceId));
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .Include(s => s.Services)
                .ThenInclude(s => s.TrainingSessions)
                .Include(s => s.Services)
                .ThenInclude(s => s.ClientServices)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (staff != null)
            {                
                foreach (var service in staff.Services)
                {                    
                    _context.ClientServices.RemoveRange(service.ClientServices);
                                        
                    _context.TrainingSessions.RemoveRange(service.TrainingSessions);
                }
                                
                _context.Services.RemoveRange(staff.Services);
                                
                _context.Staff.Remove(staff);
                                
                var user = await _userManager.FindByEmailAsync(staff.Email);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }

                await _context.SaveChangesAsync();

                TempData["Message"] = $"Staff member {staff.FullName} and all related data have been deleted";
            }

            return RedirectToPage("./Index");
        }
    }
}