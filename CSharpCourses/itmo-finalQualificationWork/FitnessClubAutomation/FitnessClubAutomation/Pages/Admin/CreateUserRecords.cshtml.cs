using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessClubAutomation.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class CreateUserRecordsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CreateUserRecordsModel(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            Results = new List<string>();
        }

        public List<string> Results { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Results = new List<string>();

            var coachUsers = await _userManager.GetUsersInRoleAsync("Coach");
            foreach (var user in coachUsers)
            {
                var existingStaff = await _context.Staff
                    .FirstOrDefaultAsync(s => s.Email == user.Email);

                if (existingStaff == null)
                {
                    var staff = new Models.Staff
                    {
                        FullName = user.UserName ?? user.Email?.Split('@')[0] ?? "Coach",
                        Email = user.Email ?? "",
                        PhoneNumber = user.PhoneNumber ?? "",
                        Position = "Coach"
                    };

                    _context.Staff.Add(staff);
                    Results.Add($"Created Staff record for {user.Email}");
                }
                else
                {
                    Results.Add($"Staff record already exists for {user.Email}");
                }
            }

            var clientUsers = await _userManager.GetUsersInRoleAsync("Client");
            foreach (var user in clientUsers)
            {
                var existingClient = await _context.Clients
                    .FirstOrDefaultAsync(c => c.Email == user.Email);

                if (existingClient == null)
                {
                    var client = new Client
                    {
                        FullName = user.UserName ?? user.Email?.Split('@')[0] ?? "Client",
                        Email = user.Email ?? "",
                        PhoneNumber = user.PhoneNumber ?? "",
                        ClientCode = "CL" + Guid.NewGuid().ToString().Substring(0, 6),
                        Status = ClientStatus.Permanent,
                        Address = "Default Address"
                    };

                    _context.Clients.Add(client);
                    Results.Add($"Created Client record for {user.Email}");
                }
                else
                {
                    Results.Add($"Client record already exists for {user.Email}");
                }
            }

            await _context.SaveChangesAsync();
            return Page();
        }
    }
}