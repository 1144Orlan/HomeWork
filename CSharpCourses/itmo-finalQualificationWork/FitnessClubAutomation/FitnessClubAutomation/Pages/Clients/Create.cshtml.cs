using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessClubAutomation.Pages.Clients
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
        public Client Client { get; set; } = default!;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public IActionResult OnGet()
        {            
            Client = new Client
            {
                ClientCode = "CL" + new Random().Next(1000, 9999).ToString()
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
            ModelState.Remove("Client.ClientServices");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Clients.Add(Client);
            await _context.SaveChangesAsync();

            var user = new IdentityUser
            {
                UserName = Client.Email,
                Email = Client.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Client");
                TempData["Message"] = $"Client created successfully with account {Client.Email} and password {Password}";
            }
            else
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                TempData["Message"] = $"Client record created, but could not create user account: {errors}";
            }

            return RedirectToPage("./Index");
        }
    }
}