using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessClubAutomation.Pages.Clients
{
    [Authorize(Roles = "Admin")]
    public class ResetPasswordModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ResetPasswordModel(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Client Client { get; set; } = default!;

        [BindProperty]
        public string NewPassword { get; set; } = string.Empty;

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

            Client = client;
            
            NewPassword = GenerateRandomPassword();

            return Page();
        }

        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            const string specialChars = "!@#$%^&*()";

            var random = new Random();
            var password = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            password += specialChars[random.Next(specialChars.Length)];

            return password;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = await _context.Clients.FindAsync(Client.Id);
            if (client == null)
            {
                return NotFound();
            }
                        
            var user = await _userManager.FindByEmailAsync(client.Email);
            if (user == null)
            {
                TempData["Message"] = $"No user account found for {client.Email}";
                return RedirectToPage("./Index");
            }
                        
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, NewPassword);

            if (result.Succeeded)
            {
                TempData["Message"] = $"Password for {client.Email} has been reset to: {NewPassword}";
            }
            else
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                TempData["Message"] = $"Failed to reset password: {errors}";
            }

            return RedirectToPage("./Index");
        }
    }
}
