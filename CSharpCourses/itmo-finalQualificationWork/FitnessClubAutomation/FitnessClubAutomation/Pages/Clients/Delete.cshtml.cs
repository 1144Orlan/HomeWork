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

namespace FitnessClubAutomation.Pages.Clients
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
        public Client Client { get; set; } = default!;

        public string Message { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FirstOrDefaultAsync(m => m.Id == id);

            if (client == null)
            {
                return NotFound();
            }
            else
            {
                Client = client;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                Client = client;

                // Find the user account associated with this client
                var user = await _userManager.FindByEmailAsync(client.Email);

                // Delete client registrations
                var clientServices = await _context.ClientServices
                    .Where(cs => cs.ClientId == client.Id)
                    .ToListAsync();

                if (clientServices.Any())
                {
                    _context.ClientServices.RemoveRange(clientServices);
                }

                // Delete the client record
                _context.Clients.Remove(Client);
                await _context.SaveChangesAsync();

                // If user account exists, remove the Client role
                if (user != null)
                {
                    var isInRole = await _userManager.IsInRoleAsync(user, "Client");
                    if (isInRole)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, "Client");
                        if (result.Succeeded)
                        {
                            Message = "Client role removed successfully";
                        }
                        else
                        {
                            Message = "Failed to remove Client role: " + string.Join(", ", result.Errors.Select(e => e.Description));
                        }
                    }
                    else
                    {
                        Message = "User is not in Client role";
                    }

                    // Check if the user still has any roles.
                    var roles = await _userManager.GetRolesAsync(user);
                    if (!roles.Any())
                    {
                        // Option: Delete the user account if it has no roles
                        var deleteResult = await _userManager.DeleteAsync(user);
                        if (deleteResult.Succeeded)
                        {
                            Message += ". User account deleted.";
                        }
                        else
                        {
                            Message += ". Failed to delete user account: " + string.Join(", ", deleteResult.Errors.Select(e => e.Description));
                        }
                    }
                }
                else
                {
                    Message = "No user account found for this client";
                }

                TempData["Message"] = Message;
            }

            return RedirectToPage("./Index");
        }
    }
}