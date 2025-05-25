using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using FitnessClubAutomation.Pages.Dashboard;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessClubAutomation.Tests
{
    public class ClientRegistrationTests
    {
        [Fact]
        public async Task OnPostRegisterForSessionAsync_ValidRegistration_CreatesClientService()
        {
            // Arrange
            var context = TestDatabaseHelper.GetInMemoryDbContext();
            var userManagerMock = MockUserManager();

            var user = new IdentityUser { UserName = "client2@test.com", Email = "client2@test.com" };
            userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            var pageModel = new ClientDashboardModel(context, userManagerMock.Object);

            // Act
            var result = await pageModel.OnPostRegisterForSessionAsync(2); // Using training session ID 2 from test data

            // Assert
            var clientService = await context.ClientServices
                .FirstOrDefaultAsync(cs => cs.ClientId == 2 && cs.TrainingSessionId == 2);

            Assert.NotNull(clientService);
            Assert.Equal(2, clientService!.ServiceId); // Service ID for training session 2

            // Check that CurrentParticipants was incremented
            var trainingSession = await context.TrainingSessions.FindAsync(2);
            Assert.NotNull(trainingSession);
            Assert.Equal(1, trainingSession!.CurrentParticipants);

            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public async Task OnPostRegisterForSessionAsync_AlreadyRegistered_ReturnsError()
        {
            // Arrange
            var context = TestDatabaseHelper.GetInMemoryDbContext();
            var userManagerMock = MockUserManager();

            var user = new IdentityUser { UserName = "client@test.com", Email = "client@test.com" };
            userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            var pageModel = new ClientDashboardModel(context, userManagerMock.Object);

            // Act
            var result = await pageModel.OnPostRegisterForSessionAsync(1); // Client is already registered for this session

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            Assert.False(pageModel.ModelState.IsValid);
        }

        private static Mock<UserManager<IdentityUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            options.Setup(o => o.Value).Returns(idOptions);
            var userValidators = new List<IUserValidator<IdentityUser>>();
            var validator = new Mock<IUserValidator<IdentityUser>>();
            userValidators.Add(validator.Object);
            var pwdValidators = new List<PasswordValidator<IdentityUser>>();
            pwdValidators.Add(new PasswordValidator<IdentityUser>());
            var userManager = new Mock<UserManager<IdentityUser>>(
                store.Object,
                options.Object,
                new PasswordHasher<IdentityUser>(),
                userValidators,
                pwdValidators,
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                new ServiceCollection().BuildServiceProvider(),
                new Mock<ILogger<UserManager<IdentityUser>>>().Object);

            return userManager;
        }
    }
}