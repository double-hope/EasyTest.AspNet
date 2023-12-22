using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.Tests.Repository
{
	[Trait("Category", "Unit")]
	public class UserRepositoryTests
	{
		private async Task<ApplicationDbContext> GetApplicationDbContext()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;
			var dbContext = new ApplicationDbContext(options);
			dbContext.Database.EnsureCreated();

			if(dbContext.Users.Count() <= 0)
			{
				var userEntity = new User { Email = "test@example.com", Name = "User Name" };
				dbContext.Users.Add(userEntity);
				await dbContext.SaveChangesAsync();
			}

			return dbContext;
		}

		[Fact]
		public async Task UserRepository_GetByEmail_ReturnUser()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
			var userRepository = new UserRepository(dbContext);
			var userEmail = dbContext.Users.First().Email;

            // Act
            var resultUser = await userRepository.GetByEmail(userEmail);

            // Assert
            Assert.NotNull(resultUser);
			Assert.Equal(userEmail, resultUser.Email);
		}
	}
}
