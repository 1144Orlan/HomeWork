using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FitnessClubAutomation.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UserRolesModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRolesModel(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            // Initialize collections to avoid null reference warnings
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

        public async Task OnGetAsync()
        {
            Users = new List<UserRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                if (user != null)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    Users.Add(new UserRoleViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName ?? string.Empty,
                        Email = user.Email ?? string.Empty,
                        Roles = userRoles.ToList()
                    });
                }
            }

            UserList = new SelectList(users, "Id", "Email");
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
    }
}