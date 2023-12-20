using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;

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
			var dbContext = await GetApplicationDbContext();
			var entities = dbContext.Set<User>().ToList();
			var repository = new Repository<User>(dbContext);

			var result = await repository.GetAll();

			Assert.NotNull(result);
			Assert.Equal(entities.Count, result.Count());
		}

		[Fact]
		public async Task Repository_GetFirstOrDefault_ReturnsFirstOrDefault()
		{
			var dbContext = await GetApplicationDbContext();
			var repository = new Repository<User>(dbContext);

			var result = await repository.GetFirstOrDefault();

			Assert.NotNull(result);
		}

		[Fact]
		public async Task Repository_Add_AddsEntity()
		{
			var dbContext = await GetApplicationDbContext();
			var repository = new Repository<User>(dbContext);

			var user = new User { Email = "user3@example.com", Name = "Test3" };
			await repository.Add(user);

			var addedUser = dbContext.Users.FirstOrDefault(u => u.Email == user.Email);

			Assert.NotNull(addedUser);
		}

		[Fact]
		public async Task Repository_AddRange_AddsRangeOfEntities()
		{
			var dbContext = await GetApplicationDbContext(); 
			var repository = new Repository<User>(dbContext);

			var users = new List<User>
			{
				new User { Email = "user3@example.com", Name = "Test3" },
				new User { Email = "user4@example.com", Name = "Test4" },
			};

			await repository.AddRange(users);

			var addedUsers = dbContext.Users.Where(u => users.Select(us => us.Email).Contains(u.Email)).ToList();

			Assert.Equal(users.Count, addedUsers.Count);
		}

		[Fact]
		public async Task Repository_Update_UpdatesEntity()
		{
			var dbContext = await GetApplicationDbContext();
			var repository = new Repository<User>(dbContext);

			var user = dbContext.Users.First();
			user.Name = "UpdatedName";

			repository.Update(user);

			var updatedUser = dbContext.Users.First(u => u.Id == user.Id);

			Assert.Equal("UpdatedName", updatedUser.Name);
		}

		[Fact]
		public async Task Repository_Remove_RemovesEntity()
		{
			var dbContext = await GetApplicationDbContext();
			var repository = new Repository<User>(dbContext);

			var user = dbContext.Users.First();

			repository.Remove(user);

			var removedUser = dbContext.Users.FirstOrDefault(u => u.Id == user.Id);

			Assert.Null(removedUser);
		}

		[Fact]
		public async Task Repository_RemoveRange_RemovesRangeOfEntities()
		{
			var dbContext = await GetApplicationDbContext();
			var repository = new Repository<User>(dbContext);

			var usersToRemove = dbContext.Users.Take(2).ToList();

			repository.RemoveRange(usersToRemove);

			var remainingUsers = dbContext.Users.Where(u => usersToRemove.Select(ur => ur.Id).Contains(u.Id)).ToList();

			Assert.Empty(remainingUsers);
		}
	}
}
