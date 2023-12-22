using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.Tests.Repository
{
	[Trait("Category", "Unit")]
	public class TestRepositoryTests
	{
		private async Task<ApplicationDbContext> GetApplicationDbContext()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;
			var dbContext = new ApplicationDbContext(options);
			dbContext.Database.EnsureCreated();

			if (dbContext.Tests.Count() <= 0)
			{
				var testEntity = new Test { Title = "Test Title", Description = "Test Description" };
				dbContext.Tests.Add(testEntity);
				await dbContext.SaveChangesAsync();
			}

			return dbContext;
		}

		[Fact]
		public async Task TestRepository_GetById_ReturnTest()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
			var testRepository = new TestRepository(dbContext);
			var testId = dbContext.Tests.First().Id;

            // Act
            var resultTest = await testRepository.GetById(testId);

            // Assert
            Assert.NotNull(resultTest);
			Assert.Equal(testId, resultTest.Id);
			Assert.Equal("Test Title", resultTest.Title);
			Assert.Equal("Test Description", resultTest.Description);
		}
	}
}
