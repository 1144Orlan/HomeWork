using FitnessClubAutomation.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessClubAutomation.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UserRolesModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UserRolesModel(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;            
            Users = new List<UserRoleViewModel>();
            Input = new InputModel();
        }

        public class UserRoleViewModel
        {
            public string UserId { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public List<string> Roles { get; set; } = new List<string>();
        }

        public List<UserRoleViewModel> Users { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string UserId { get; set; } = string.Empty;

            [Required]
            public string RoleName { get; set; } = string.Empty;
        }

        public SelectList? UserList { get; set; }
        public SelectList? RoleList { get; set; }

        [TempData]
        public string StatusMessage { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            Users = new List<UserRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();
                        
            var clients = await _context.Clients.ToListAsync();
            var staff = await _context.Staff.ToListAsync();

            foreach (var user in users)
            {
                if (user != null)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    
                    string displayName = user.UserName ?? string.Empty;
                    
                    var client = clients.FirstOrDefault(c => c.Email == user.Email);
                    if (client != null && !string.IsNullOrEmpty(client.FullName))
                    {
                        displayName = client.FullName;
                    }
                    else
                    {                        
                        var staffMember = staff.FirstOrDefault(s => s.Email == user.Email);
                        if (staffMember != null && !string.IsNullOrEmpty(staffMember.FullName))
                        {
                            displayName = staffMember.FullName;
                        }
                    }

                    Users.Add(new UserRoleViewModel
                    {
                        UserId = user.Id,
                        UserName = displayName,
                        Email = user.Email ?? string.Empty,
                        Roles = userRoles.ToList()
                    });
                }
            }
            
            var userSelectList = users.Select(u => {
                string displayName = u.UserName ?? string.Empty;
                                
                var client = clients.FirstOrDefault(c => c.Email == u.Email);
                if (client != null && !string.IsNullOrEmpty(client.FullName))
                {
                    displayName = client.FullName;
                }
                else
                {                    
                    var staffMember = staff.FirstOrDefault(s => s.Email == u.Email);
                    if (staffMember != null && !string.IsNullOrEmpty(staffMember.FullName))
                    {
                        displayName = staffMember.FullName;
                    }
                }

                return new { Id = u.Id, DisplayName = $"{displayName} ({u.Email})" };
            });

            UserList = new SelectList(userSelectList, "Id", "DisplayName");
            RoleList = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
        }

        public async Task<IActionResult> OnPostAddToRoleAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var user = await _userManager.FindByIdAsync(Input.UserId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found");
                await OnGetAsync();
                return Page();
            }

            var role = await _roleManager.FindByNameAsync(Input.RoleName);
            if (role == null)
            {
                ModelState.AddModelError(string.Empty, "Role not found");
                await OnGetAsync();
                return Page();
            }

            var result = await _userManager.AddToRoleAsync(user, Input.RoleName);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await OnGetAsync();
                return Page();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveFromRoleAsync(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
                        
            if (!string.IsNullOrEmpty(user.Email))
            {
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == user.Email);
                if (client != null)
                {                    
                    var clientServices = await _context.ClientServices
                        .Where(cs => cs.ClientId == client.Id)
                        .ToListAsync();

                    if (clientServices.Any())
                    {
                        _context.ClientServices.RemoveRange(clientServices);
                    }
                                        
                    _context.Clients.Remove(client);
                    await _context.SaveChangesAsync();
                    StatusMessage = "Client record deleted. ";
                }
                                
                var staff = await _context.Staff.FirstOrDefaultAsync(s => s.Email == user.Email);
                if (staff != null)
                {                    
                    _context.Staff.Remove(staff);
                    await _context.SaveChangesAsync();
                    StatusMessage += "Staff record deleted. ";
                }
            }
                        
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                StatusMessage += $"User account {user.Email} has been deleted.";
            }
            else
            {
                StatusMessage += $"Error deleting user account: {string.Join(", ", result.Errors.Select(e => e.Description))}";
            }

            return RedirectToPage();
        }
    }
}