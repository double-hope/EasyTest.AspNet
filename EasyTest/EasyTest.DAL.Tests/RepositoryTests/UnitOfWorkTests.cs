using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.Tests.Repository
{
	[Trait("Category", "Unit")]
	public class UnitOfWorkTests
    {
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
        public async Task UnitOfWork_Save_SavesChanges()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
            var unitOfWork = new UnitOfWork(dbContext);

            var question = new Question() { Title = "Title", Text = "Text" };
            var test = new Test() { Title = "Title", Description = "Description" };

            dbContext.Attach(question);
            dbContext.Attach(test);

            // Act
            await unitOfWork.Save();

            // Assert
            Assert.Equal(EntityState.Unchanged, dbContext.Entry(question).State);
            Assert.Equal(EntityState.Unchanged, dbContext.Entry(test).State);
        }
    }
}
