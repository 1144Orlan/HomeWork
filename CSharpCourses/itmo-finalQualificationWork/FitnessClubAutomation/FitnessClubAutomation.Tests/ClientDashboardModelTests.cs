using FitnessClubAutomation.Data;
using FitnessClubAutomation.Models;
using FitnessClubAutomation.Pages.Dashboard;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessClubAutomation.Tests
{
    public class ClientDashboardModelTests
    {
        [Fact]
        public async Task OnGetAsync_WithValidUser_LoadsClientData()
        {
            // Arrange
            var context = TestDatabaseHelper.GetInMemoryDbContext();

            // Mock UserManager
            var userManagerMock = MockUserManager();
            var user = new IdentityUser { UserName = "client@test.com", Email = "client@test.com" };
            userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            var model = new ClientDashboardModel(context, userManagerMock.Object);

            // Act
            await model.OnGetAsync();

            // Assert
            Assert.NotNull(model.Client);
            Assert.Equal("Test Client", model.Client!.FullName);
            Assert.Single(model.ClientServices);
            Assert.Single(model.RegisteredServiceIds);
            Assert.Equal(2, model.AvailableServices.Count); // All services should be shown
            Assert.Single(model.AvailableTrainingSessions); // Only future sessions for services not registered
        }

        [Fact]
        public async Task OnGetAsync_WithInvalidUser_DoesNotLoadData()
        {
            // Arrange
            var context = TestDatabaseHelper.GetInMemoryDbContext();

            // Mock UserManager
            var userManagerMock = MockUserManager();
            userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((IdentityUser?)null);

            var model = new ClientDashboardModel(context, userManagerMock.Object);

            // Act
            await model.OnGetAsync();

            // Assert
            Assert.Null(model.Client);
            Assert.Empty(model.ClientServices);
            Assert.Empty(model.RegisteredServiceIds);
            Assert.Empty(model.AvailableServices);
            Assert.Empty(model.AvailableTrainingSessions);
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