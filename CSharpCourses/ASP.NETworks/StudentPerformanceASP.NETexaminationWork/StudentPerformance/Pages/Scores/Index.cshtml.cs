using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentPerformance.Data;
using StudentPerformance.Models;

namespace StudentPerformance.Pages.Scores
{
    public class IndexModel : PageModel
    {
        private readonly StudentPerformance.Data.ApplicationDbContext _context;

        public IndexModel(StudentPerformance.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Score> Score { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Score = await _context.Scores
                .Include(s => s.Student).ToListAsync();
        }
    }
}
