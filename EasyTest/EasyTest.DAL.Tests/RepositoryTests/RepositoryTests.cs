using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.Tests.Repository
{
	public class RepositoryTests
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
				dbContext.Users.Add(new User { Email = "user1@example.com", Name = "Test1" });
				dbContext.Users.Add(new User { Email = "user2@example.com", Name = "Test2" });
				await dbContext.SaveChangesAsync();
			}

			return dbContext;
		}

		[Fact]
		public async Task Repository_GetAll_ReturnsAllEntities()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
			var entities = dbContext.Set<User>().ToList();
			var repository = new Repository<User>(dbContext);

            // Act
            var result = await repository.GetAll();

            // Assert
            Assert.NotNull(result);
			Assert.Equal(entities.Count, result.Count());
		}

		[Fact]
		public async Task Repository_GetFirstOrDefault_ReturnsFirstOrDefault()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
			var repository = new Repository<User>(dbContext);

			var result = await repository.GetFirstOrDefault();

            // Assert
            Assert.NotNull(result);
		}

		[Fact]
		public async Task Repository_Add_AddsEntity()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
			var repository = new Repository<User>(dbContext);

            // Act
            var user = new User { Email = "user3@example.com", Name = "Test3" };
			await repository.Add(user);
            await dbContext.SaveChangesAsync();

            var addedUser = dbContext.Users.FirstOrDefault(u => u.Email == user.Email);

            // Assert
            Assert.NotNull(addedUser);
		}

		[Fact]
		public async Task Repository_AddRange_AddsRangeOfEntities()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext(); 
			var repository = new Repository<User>(dbContext);

			var users = new List<User>
			{
				new User { Email = "user3@example.com", Name = "Test3" },
				new User { Email = "user4@example.com", Name = "Test4" },
			};

            // Act
            await repository.AddRange(users);
            await dbContext.SaveChangesAsync();

            var addedUsers = dbContext.Users.Where(u => users.Select(us => us.Email).Contains(u.Email)).ToList();

            // Assert
            Assert.Equal(users.Count, addedUsers.Count);
		}

		[Fact]
		public async Task Repository_Update_UpdatesEntity()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
			var repository = new Repository<User>(dbContext);

			var user = dbContext.Users.First();
			user.Name = "UpdatedName";

            // Act
            repository.Update(user);

			var updatedUser = dbContext.Users.First(u => u.Id == user.Id);

            // Assert
            Assert.Equal("UpdatedName", updatedUser.Name);
		}

		[Fact]
		public async Task Repository_Remove_RemovesEntity()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
			var repository = new Repository<User>(dbContext);

			var user = dbContext.Users.First();

            // Act
            repository.Remove(user);
			await dbContext.SaveChangesAsync();

			var removedUser = dbContext.Users.FirstOrDefault(u => u.Id == user.Id);

            // Assert
            Assert.Null(removedUser);
		}

		[Fact]
		public async Task Repository_RemoveRange_RemovesRangeOfEntities()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
			var repository = new Repository<User>(dbContext);

			var usersToRemove = dbContext.Users.Take(2).ToList();

            // Act
            repository.RemoveRange(usersToRemove);
            await dbContext.SaveChangesAsync();

            var remainingUsers = dbContext.Users.Where(u => usersToRemove.Select(ur => ur.Id).Contains(u.Id)).ToList();

            // Assert
            Assert.Empty(remainingUsers);
		}
	}
}
