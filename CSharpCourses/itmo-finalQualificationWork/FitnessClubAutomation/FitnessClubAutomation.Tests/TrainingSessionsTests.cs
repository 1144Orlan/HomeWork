using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using FitnessClubAutomation.Pages.TrainingSessions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FitnessClubAutomation.Tests
{
    public class TrainingSessionsTests
    {
        [Fact]
        public async Task OnPostAsync_ValidTrainingSession_CreatesTrainingSession()
        {
            // Arrange
            var context = TestDatabaseHelper.GetInMemoryDbContext();
            var pageModel = new CreateModel(context);

            var trainingSession = new TrainingSession
            {
                ServiceId = 1, // Using existing service from test data
                DateTime = DateTime.Now.AddDays(1),
                MaxParticipants = 15,
                CurrentParticipants = 0
            };

            pageModel.TrainingSession = trainingSession;

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            var createdSession = await context.TrainingSessions
                .FirstOrDefaultAsync(ts => ts.ServiceId == 1 && ts.MaxParticipants == 15);

            Assert.NotNull(createdSession);
            Assert.Equal(15, createdSession.MaxParticipants);
            Assert.Equal(0, createdSession.CurrentParticipants);
            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public async Task OnGetAsync_ExistingId_ReturnsTrainingSessionWithService()
        {
            // Arrange
            var context = TestDatabaseHelper.GetInMemoryDbContext();
            var pageModel = new DetailsModel(context);

            // Act
            var result = await pageModel.OnGetAsync(1); // Using existing training session ID from test data

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.NotNull(pageModel.TrainingSession);
            Assert.Equal(1, pageModel.TrainingSession.Id);
            Assert.Equal("Group Training", pageModel.TrainingSession.Service.Name);
        }

        [Fact]
        public async Task OnGetAsync_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var context = TestDatabaseHelper.GetInMemoryDbContext();
            var pageModel = new DetailsModel(context);

            // Act
            var result = await pageModel.OnGetAsync(999); // Non-existing ID

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}