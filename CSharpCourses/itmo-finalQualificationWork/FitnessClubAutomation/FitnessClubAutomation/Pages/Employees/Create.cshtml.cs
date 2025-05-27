using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessClubAutomation.Pages.Employees
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Staff Staff { get; set; } = default!;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            Staff = new Staff
            {
                Position = "Coach"
            };

            Password = GenerateRandomPassword();

            return Page();
        }

        private string GenerateRandomPassword()
        {
            const string uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()";

            var random = new Random();
                        
            var password = new string(Enumerable.Repeat(uppercaseLetters, 1)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            
            password += new string(Enumerable.Repeat(lowercaseLetters, 1)
                .Select(s => s[random.Next(s.Length)]).ToArray());
                        
            password += new string(Enumerable.Repeat(uppercaseLetters + lowercaseLetters, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());
                        
            password += new string(Enumerable.Repeat(digits, 2)
                .Select(s => s[random.Next(s.Length)]).ToArray());
                        
            password += specialChars[random.Next(specialChars.Length)];

            return password;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Staff.Services");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _context.Staff.Add(Staff);
                await _context.SaveChangesAsync();

                var user = new IdentityUser
                {
                    UserName = Staff.Email,
                    Email = Staff.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Coach");
                    TempData["Message"] = $"Staff member created successfully with account {Staff.Email} and password {Password}";
                }
                else
                {
                    string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    TempData["Message"] = $"Staff record created, but could not create user account: {errors}";
                }

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Exception: {ex.Message}";
                return Page();
            }
        }
    }
}