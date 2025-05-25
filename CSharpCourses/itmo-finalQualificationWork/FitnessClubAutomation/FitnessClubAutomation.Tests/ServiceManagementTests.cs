using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using FitnessClubAutomation.Pages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FitnessClubAutomation.Tests
{
    public class ServiceManagementTests
    {
        [Fact]
        public async Task OnPostAsync_ValidService_CreatesService()
        {
            // Arrange
            var context = TestDatabaseHelper.GetInMemoryDbContext();
            var pageModel = new CreateModel(context);

            var service = new Service
            {
                Name = "New Test Service",
                Cost = 150.00m,
                DurationMinutes = 90,
                Type = TrainingType.Group,
                StaffId = 1 // Using existing staff from test data
            };

            pageModel.Service = service;

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            var createdService = await context.Services
                .FirstOrDefaultAsync(s => s.Name == "New Test Service");

            Assert.NotNull(createdService);
            Assert.Equal(150.00m, createdService!.Cost);
            Assert.Equal(90, createdService.DurationMinutes);
            Assert.Equal(TrainingType.Group, createdService.Type);
            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public async Task OnGetAsync_ExistingId_ReturnsServiceWithCoach()
        {
            // Arrange
            var context = TestDatabaseHelper.GetInMemoryDbContext();
            var pageModel = new DetailsModel(context);

            // Act
            var result = await pageModel.OnGetAsync(1); // Using existing service ID from test data

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.NotNull(pageModel.Service);
            Assert.Equal(1, pageModel.Service.Id);
            Assert.Equal("Group Training", pageModel.Service.Name);
            Assert.NotNull(pageModel.Service.Coach);
            Assert.Equal("Test Coach", pageModel.Service.Coach.FullName);
        }

        [Fact]
        public async Task OnPostAsync_EditService_UpdatesService()
        {
            // Arrange
            var context = TestDatabaseHelper.GetInMemoryDbContext();

            // First, get the service and modify it
            var service = await context.Services.FindAsync(1);
            Assert.NotNull(service);

            service!.Name = "Updated Service Name";
            service.Cost = 125.00m;

            // Create a new EditModel and set the Service property
            var pageModel = new EditModel(context);
            pageModel.Service = service;

            // Remove validation errors for navigation properties
            pageModel.ModelState.Remove("Service.Coach");
            pageModel.ModelState.Remove("Service.ClientServices");
            pageModel.ModelState.Remove("Service.TrainingSessions");

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            var updatedService = await context.Services.FindAsync(1);
            Assert.NotNull(updatedService);
            Assert.Equal("Updated Service Name", updatedService!.Name);
            Assert.Equal(125.00m, updatedService.Cost);
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}