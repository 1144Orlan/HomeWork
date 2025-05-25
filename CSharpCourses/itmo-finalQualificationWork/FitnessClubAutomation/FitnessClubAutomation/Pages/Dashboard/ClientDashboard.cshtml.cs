using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public List<TrainingSession> AvailableTrainingSessions { get; set; } = new List<TrainingSession>();
        public List<int> RegisteredServiceIds { get; set; } = new List<int>();
        public List<int> RegisteredTrainingSessionIds { get; set; } = new List<int>();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                Client = await _context.Clients
                    .FirstOrDefaultAsync(c => c.Email == user.Email);

                if (Client != null)
                {                    
                    var allClientServices = await _context.ClientServices
                        .Include(cs => cs.Service)
                        .ThenInclude(s => s.Coach)
                        .Include(cs => cs.TrainingSession)
                        .Where(cs => cs.ClientId == Client.Id)
                        .ToListAsync();                    
                    
                    ClientServices = allClientServices
                        .Where(cs => cs.TrainingSessionId == null ||
                               (cs.TrainingSession != null && cs.TrainingSession.DateTime > DateTime.Now))
                        .ToList();                    
                    
                    var oldRegistrations = allClientServices
                        .Where(cs => cs.TrainingSessionId == null)
                        .ToList();

                    if (oldRegistrations.Any())
                    {
                        _context.ClientServices.RemoveRange(oldRegistrations);
                        await _context.SaveChangesAsync();
                        
                        ClientServices = ClientServices
                            .Where(cs => cs.TrainingSessionId != null)
                            .ToList();
                    }

                    RegisteredTrainingSessionIds = await _context.ClientServices
                        .Where(cs => cs.ClientId == Client.Id && cs.TrainingSessionId.HasValue)
                        .Select(cs => cs.TrainingSessionId!.Value)
                        .ToListAsync();

                    RegisteredServiceIds = ClientServices.Select(cs => cs.ServiceId).ToList();
                                        
                    AvailableServices = await _context.Services
                        .Include(s => s.Coach)
                        .ToListAsync();
                    
                    AvailableTrainingSessions = await _context.TrainingSessions
                        .Include(t => t.Service)
                        .ThenInclude(s => s.Coach)
                        .Where(t =>
                            !RegisteredTrainingSessionIds.Contains(t.Id) &&
                            t.DateTime > DateTime.Now &&
                            (!t.MaxParticipants.HasValue || t.CurrentParticipants < t.MaxParticipants.Value))
                        .OrderBy(t => t.DateTime)
                        .ToListAsync();
                }
            }
        }

        public async Task<IActionResult> OnPostRegisterForSessionAsync(int trainingSessionId)
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

            var trainingSession = await _context.TrainingSessions
                .Include(t => t.Service)
                .FirstOrDefaultAsync(t => t.Id == trainingSessionId);

            if (trainingSession == null)
            {
                return NotFound("Training session not found");
            }

            var existingRegistration = await _context.ClientServices
                .AnyAsync(cs => cs.ClientId == client.Id && cs.TrainingSessionId == trainingSessionId);

            if (existingRegistration)
            {
                ModelState.AddModelError(string.Empty, "You are already registered for this training session");
                return RedirectToPage();
            }

            if (trainingSession.MaxParticipants.HasValue &&
                trainingSession.CurrentParticipants >= trainingSession.MaxParticipants.Value)
            {
                ModelState.AddModelError(string.Empty, "This session is already full");
                return RedirectToPage();
            }

            var clientService = new ClientService
            {
                ClientId = client.Id,
                ServiceId = trainingSession.ServiceId,
                TrainingSessionId = trainingSessionId,
                RegistrationDate = DateTime.Now
            };

            _context.ClientServices.Add(clientService);

            trainingSession.CurrentParticipants++;
            _context.TrainingSessions.Update(trainingSession);

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}