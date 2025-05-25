using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessClubAutomation.Pages.TrainingSessions
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public TrainingSession TrainingSession { get; set; } = default!;
        public List<Client> RegisteredClients { get; set; } = new List<Client>();

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
                        
            var clientServices = await _context.ClientServices
                .Include(cs => cs.Client)
                .Where(cs => cs.TrainingSessionId == TrainingSession.Id)
                .ToListAsync();

            RegisteredClients = clientServices.Select(cs => cs.Client).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveClientAsync(int clientId)
        {
            var trainingSessionId = RouteData.Values["id"]?.ToString();
            if (string.IsNullOrEmpty(trainingSessionId) || !int.TryParse(trainingSessionId, out int id))
            {
                return NotFound();
            }

            var trainingSession = await _context.TrainingSessions
                .Include(t => t.Service)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainingSession == null)
            {
                return NotFound();
            }
            
            var clientService = await _context.ClientServices
                .FirstOrDefaultAsync(cs => cs.ClientId == clientId && cs.TrainingSessionId == id);

            if (clientService != null)
            {
                _context.ClientServices.Remove(clientService);

                if (trainingSession.CurrentParticipants > 0)
                {
                    trainingSession.CurrentParticipants--;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { id });
        }
    }
}