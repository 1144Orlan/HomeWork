using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentPerformance.Data;
using System.Text;
using StudentPerformance.Models;

namespace StudentPerformance.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DashboardModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public List<StudentViewModel> TopStudents { get; set; }
        public List<StudentViewModel> BottomStudents { get; set; }

        public async Task OnGetAsync()
        {
            var students = await _context.Students
                .Include(s => s.Scores)
                .Select(s => new StudentViewModel
                {
                    Name = s.Name,
                    TotalPoints = s.Scores.Sum(score => score.Points)
                })
                .OrderByDescending(s => s.TotalPoints)
                .ToListAsync();

            TopStudents = students.Take(5).ToList();
            BottomStudents = students.TakeLast(5).Reverse().ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var students = await _context.Students
                .Include(s => s.Scores)
                .ToListAsync();

            var sb = new StringBuilder();
            sb.AppendLine("Student Performance Report");
            sb.AppendLine("==========================");

            foreach (var student in students)
            {
                sb.AppendLine($"Name: {student.Name}");
                sb.AppendLine($"Student ID: {student.StudentId}");
                sb.AppendLine("Scores:");
                foreach (var score in student.Scores)
                {
                    sb.AppendLine($"  {score.Subject}: {score.Points}");
                }
                sb.AppendLine($"Total Points: {student.Scores.Sum(s => s.Points)}");
                sb.AppendLine();
            }

            var filePath = Path.Combine(_environment.WebRootPath, "student_performance.txt");
            await System.IO.File.WriteAllTextAsync(filePath, sb.ToString());

            return RedirectToPage("./Dashboard");
        }
    }

    public class StudentViewModel
    {
        public string Name { get; set; }
        public int TotalPoints { get; set; }
    }

}
