using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FitnessClubAutomation.Tests
{
    public static class TestDatabaseHelper
    {
        public static ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);
            SeedDatabase(context);
            return context;
        }

        private static void SeedDatabase(ApplicationDbContext context)
        {
            // Add test clients
            var client1 = new Client
            {
                Id = 1,
                FullName = "Test Client",
                Email = "client@test.com",
                PhoneNumber = "1234567890",
                ClientCode = "CL1234",
                Address = "Test Address",
                Status = ClientStatus.Permanent
            };

            var client2 = new Client
            {
                Id = 2,
                FullName = "Another Client",
                Email = "client2@test.com",
                PhoneNumber = "0987654321",
                ClientCode = "CL5678",
                Address = "Another Address",
                Status = ClientStatus.OneTime
            };

            // Add test staff
            var staff1 = new Staff
            {
                Id = 1,
                FullName = "Test Coach",
                Email = "coach@test.com",
                PhoneNumber = "1122334455",
                Position = "Coach"
            };

            // Add test services
            var service1 = new Service
            {
                Id = 1,
                Name = "Group Training",
                Cost = 100.00m,
                DurationMinutes = 60,
                Type = TrainingType.Group,
                StaffId = 1
            };

            var service2 = new Service
            {
                Id = 2,
                Name = "Individual Training",
                Cost = 200.00m,
                DurationMinutes = 45,
                Type = TrainingType.Individual,
                StaffId = 1
            };

            // Add test training sessions
            var futureDate = DateTime.Now.AddDays(1);
            var pastDate = DateTime.Now.AddDays(-1);

            var trainingSession1 = new TrainingSession
            {
                Id = 1,
                ServiceId = 1,
                DateTime = futureDate,
                MaxParticipants = 10,
                CurrentParticipants = 1
            };

            var trainingSession2 = new TrainingSession
            {
                Id = 2,
                ServiceId = 2,
                DateTime = futureDate,
                MaxParticipants = 1,
                CurrentParticipants = 0
            };

            var trainingSession3 = new TrainingSession
            {
                Id = 3,
                ServiceId = 1,
                DateTime = pastDate,
                MaxParticipants = 10,
                CurrentParticipants = 1
            };

            // Add test client services
            var clientService1 = new ClientService
            {
                Id = 1,
                ClientId = 1,
                ServiceId = 1,
                TrainingSessionId = 1,
                RegistrationDate = DateTime.Now.AddDays(-2)
            };

            // Add entities to context
            context.Clients.AddRange(client1, client2);
            context.Staff.Add(staff1);
            context.Services.AddRange(service1, service2);
            context.TrainingSessions.AddRange(trainingSession1, trainingSession2, trainingSession3);
            context.ClientServices.Add(clientService1);

            context.SaveChanges();
        }
    }
}