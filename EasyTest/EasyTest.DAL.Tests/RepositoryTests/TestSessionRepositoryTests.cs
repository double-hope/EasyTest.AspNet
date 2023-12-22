using EasyTest.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using EasyTest.Shared.Enums;
using EasyTest.DAL.Repository;

namespace EasyTest.DAL.Tests.Repository
{
	[Trait("Category", "Unit")]
	public class TestSessionRepositoryTests
	{
		private async Task<ApplicationDbContext> GetApplicationDbContext()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;
			var dbContext = new ApplicationDbContext(options);
			dbContext.Database.EnsureCreated();

			var userId = Guid.NewGuid();
			var testId = Guid.NewGuid();

			if (dbContext.TestSessions.Count() <= 0)
			{
                dbContext.TestSessions.Add(new TestSession()
				{
					UserId = userId,
					TestId = testId,
					Status = TestStatus.InProgress
				});
				dbContext.TestSessions.Add(new TestSession()
				{
					UserId = userId,
					TestId = testId,
					Status = TestStatus.Passed
				});
				dbContext.TestSessions.Add(new TestSession()
				{
					UserId = userId,
					TestId = testId,
					Status = TestStatus.Passed
				});
				dbContext.Tests.Add(new Test()
				{
					Id = testId,
					Title = "Title",
					Description = "Description"
				});
				dbContext.Questions.Add(new Question()
				{
					Title = "Title",
					Text = "Text"
				});
				await dbContext.SaveChangesAsync();
			}

			return dbContext;
		}

		[Fact]
		public async Task TestSessionRepository_GetInProgressSession_ReturnInProgressSession()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
			var testSessionRepository = new TestSessionRepository(dbContext);
			var userId = dbContext.TestSessions.First().UserId;
			var testId = dbContext.TestSessions.First().TestId;

            // Act
            var resultSession = await testSessionRepository.GetInProgressSession(userId, testId);

            // Assert
            Assert.NotNull(resultSession);
			Assert.Equal(TestStatus.InProgress, resultSession.Status);
		}

		[Fact]
		public async Task TestSessionRepository_GetSession_ReturnSession()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
			var testSessionRepository = new TestSessionRepository(dbContext);
			var sessionId = dbContext.TestSessions.First().Id;

            // Act
            var resultSession = await testSessionRepository.GetSession(sessionId);

            // Assert
            Assert.NotNull(resultSession);
			Assert.Equal(sessionId, resultSession.Id);
		}

		[Fact]
		public async Task TestSessionRepository_GetSession_ReturnNull ()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
			var testSessionRepository = new TestSessionRepository(dbContext);
			var sessionId = Guid.NewGuid();

            // Act
            var resultSession = await testSessionRepository.GetSession(sessionId);

            // Assert
            Assert.Null(resultSession);
		}

		[Fact]
		public async Task TestSessionRepository_GetAllUserSessions_ReturnAllSessions()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
			var testSessionRepository = new TestSessionRepository(dbContext);
			var userId = dbContext.TestSessions.First().UserId;
			var testId = dbContext.TestSessions.First().TestId;

            // Act
            var resultSessions = await testSessionRepository.GetAllUserSessions(userId, testId);

            // Assert
            Assert.NotNull(resultSessions);
			Assert.Equal(3, resultSessions.Count);
		}

		[Fact]
		public async Task TestSessionRepository_GetAllUserSessions_ReturnEmpty()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
			var testSessionRepository = new TestSessionRepository(dbContext);
			var userId = Guid.NewGuid();
			var testId = Guid.NewGuid();

            // Act
            var resultSessions = await testSessionRepository.GetAllUserSessions(userId, testId);

            // Assert
            Assert.NotNull(resultSessions);
			Assert.Empty(resultSessions);
		}
	}
}
