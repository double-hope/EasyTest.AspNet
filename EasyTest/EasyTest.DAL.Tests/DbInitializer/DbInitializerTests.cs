using EasyTest.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using FakeItEasy;

namespace EasyTest.DAL.Tests.DbInitializer
{
    public class DbInitializerTests
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public DbInitializerTests()
        {
            _userManager = A.Fake<UserManager<User>>();
            _roleManager = A.Fake<RoleManager<IdentityRole<Guid>>>();
        }
        private async Task<ApplicationDbContext> GetApplicationDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        [Fact]
        public async Task Initialize_WithExistingRoles_DoesNotCreateRoles()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();

            var initializer = new DAL.DbInitializer.DbInitializer(_userManager, _roleManager, dbContext);

            Mock.Get(_roleManager).Setup(r => r.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            initializer.Initialize();

            // Assert
            Mock.Get(_roleManager).Verify(r => r.CreateAsync(It.IsAny<IdentityRole<Guid>>()), Moq.Times.Never);
        }
    }
}
