using FitnessClubAutomation.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessClubAutomation.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ClientService> ClientServices { get; set; }
        public DbSet<TrainingSession> TrainingSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
                        
            modelBuilder.Entity<ClientService>()
                .HasOne(cs => cs.Client)
                .WithMany(c => c.ClientServices)
                .HasForeignKey(cs => cs.ClientId);

            modelBuilder.Entity<ClientService>()
                .HasOne(cs => cs.Service)
                .WithMany(s => s.ClientServices)
                .HasForeignKey(cs => cs.ServiceId);
                        
            modelBuilder.Entity<ClientService>()
                .HasOne(cs => cs.TrainingSession)
                .WithMany()
                .HasForeignKey(cs => cs.TrainingSessionId)
                .IsRequired(false);

            modelBuilder.Entity<Service>()
                .HasOne(s => s.Coach)
                .WithMany(c => c.Services)
                .HasForeignKey(s => s.StaffId);

            modelBuilder.Entity<Service>()
                .Property(s => s.Cost)
                .HasPrecision(10, 2);

            modelBuilder.Entity<TrainingSession>()
                .HasOne(ts => ts.Service)
                .WithMany(s => s.TrainingSessions)
                .HasForeignKey(ts => ts.ServiceId);
        }
    }
}