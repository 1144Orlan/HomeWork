using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FitnessClubAutomation.Pages.Dashboard
{
    [Authorize(Roles = "Client")]
    public class ClientDashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ClientDashboardModel(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Client? Client { get; set; }
        public List<ClientService> ClientServices { get; set; } = new List<ClientService>();
        public List<Service> AvailableServices { get; set; } = new List<Service>();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {                
                Client = await _context.Clients
                    .FirstOrDefaultAsync(c => c.Email == user.Email);

                if (Client != null)
                {                    
                    ClientServices = await _context.ClientServices
                        .Include(cs => cs.Service)
                        .ThenInclude(s => s.Coach)
                        .Where(cs => cs.ClientId == Client.Id)
                        .ToListAsync();
                    
                    var registeredServiceIds = ClientServices.Select(cs => cs.ServiceId).ToList();
                    AvailableServices = await _context.Services
                        .Include(s => s.Coach)
                        .Where(s => !registeredServiceIds.Contains(s.Id))
                        .ToListAsync();
                }
            }
        }

        public async Task<IActionResult> OnPostAsync(int serviceId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Email == user.Email);

            if (client == null)
            {
                return NotFound("Client profile not found");
            }

            var service = await _context.Services.FindAsync(serviceId);
            if (service == null)
            {
                return NotFound("Service not found");
            }
            
            var existingRegistration = await _context.ClientServices
                .AnyAsync(cs => cs.ClientId == client.Id && cs.ServiceId == serviceId);

            if (existingRegistration)
            {
                ModelState.AddModelError(string.Empty, "You are already registered for this service");
                return Page();
            }
            
            var clientService = new ClientService
            {
                ClientId = client.Id,
                ServiceId = serviceId,
                RegistrationDate = DateTime.Now
            };

            _context.ClientServices.Add(clientService);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}