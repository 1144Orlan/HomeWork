using Microsoft.EntityFrameworkCore;
using StudentPerformance.Models;
using static System.Formats.Asn1.AsnWriter;
namespace StudentPerformance.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Score> Scores { get; set; }

    }
}
