using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessClubAutomation.Pages.TrainingSessions
{
    [Authorize(Roles = "Admin")]
    public class AddClientModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddClientModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int ClientId { get; set; }

        public TrainingSession TrainingSession { get; set; } = default!;
        public SelectList? AvailableClients { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingSession = await _context.TrainingSessions
                .Include(t => t.Service)
                .ThenInclude(s => s.Coach)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainingSession == null)
            {
                return NotFound();
            }

            TrainingSession = trainingSession;
                        
            var registeredClientIds = await _context.ClientServices
                .Where(cs => cs.TrainingSessionId == TrainingSession.Id)
                .Select(cs => cs.ClientId)
                .ToListAsync();
                        
            var availableClients = await _context.Clients
                .Where(c => !registeredClientIds.Contains(c.Id))
                .ToListAsync();

            AvailableClients = new SelectList(availableClients, "Id", "FullName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var trainingSession = await _context.TrainingSessions
                .Include(t => t.Service)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainingSession == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(ClientId);
            if (client == null)
            {
                return NotFound();
            }
                        
            var existingRegistration = await _context.ClientServices
                .AnyAsync(cs => cs.ClientId == ClientId && cs.TrainingSessionId == id);

            if (existingRegistration)
            {
                ModelState.AddModelError(string.Empty, "Client is already registered for this training session");
                                
                var registeredClientIds = await _context.ClientServices
                    .Where(cs => cs.TrainingSessionId == id)
                    .Select(cs => cs.ClientId)
                    .ToListAsync();

                var availableClients = await _context.Clients
                    .Where(c => !registeredClientIds.Contains(c.Id))
                    .ToListAsync();

                AvailableClients = new SelectList(availableClients, "Id", "FullName");

                TrainingSession = trainingSession;
                return Page();
            }

            if (trainingSession.MaxParticipants.HasValue &&
                trainingSession.CurrentParticipants >= trainingSession.MaxParticipants.Value)
            {
                ModelState.AddModelError(string.Empty, "This session is already full");
                                
                var registeredClientIds = await _context.ClientServices
                    .Where(cs => cs.TrainingSessionId == id)
                    .Select(cs => cs.ClientId)
                    .ToListAsync();

                var availableClients = await _context.Clients
                    .Where(c => !registeredClientIds.Contains(c.Id))
                    .ToListAsync();

                AvailableClients = new SelectList(availableClients, "Id", "FullName");

                TrainingSession = trainingSession;
                return Page();
            }
                        
            var clientService = new ClientService
            {
                ClientId = ClientId,
                ServiceId = trainingSession.ServiceId,
                TrainingSessionId = id,
                RegistrationDate = DateTime.Now
            };

            _context.ClientServices.Add(clientService);

            trainingSession.CurrentParticipants++;
            _context.TrainingSessions.Update(trainingSession);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { id });
        }
    }
}