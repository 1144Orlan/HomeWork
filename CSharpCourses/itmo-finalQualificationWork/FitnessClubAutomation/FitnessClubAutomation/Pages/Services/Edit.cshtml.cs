using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessClubAutomation.Pages.Services
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly FitnessClubAutomation.Data.ApplicationDbContext _context;

        public EditModel(FitnessClubAutomation.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Service Service { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }
            Service = service;
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "FullName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            // Remove validation errors for navigation properties
            ModelState.Remove("Service.Coach");
            ModelState.Remove("Service.ClientServices");
            ModelState.Remove("Service.TrainingSessions");

            // Fix decimal parsing issues
            if (ModelState.TryGetValue("Service.Cost", out var costModelState) && costModelState.Errors.Count > 0)
            {
                // Try to parse the cost value with invariant culture
                if (decimal.TryParse(Request.Form["Service.Cost"], NumberStyles.Any,
                    CultureInfo.InvariantCulture, out var costValue))
                {
                    Service.Cost = costValue;
                    ModelState.Remove("Service.Cost");
                }
            }

            if (!ModelState.IsValid)
            {
                ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "FullName");
                return Page();
            }

            try
            {
                _context.Attach(Service).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(Service.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}