using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.Tests.Repository
{
	public class SessionQuestionRepositoryTests
	{
		private async Task<ApplicationDbContext> GetApplicationDbContext()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;
			var dbContext = new ApplicationDbContext(options);
			dbContext.Database.EnsureCreated();

			if (dbContext.SessionQuestions.Count() <= 0)
			{
				var testSessionId = Guid.NewGuid();
				var questionIds = new List<Guid>
			{
				Guid.NewGuid(),
				Guid.NewGuid(),
				Guid.NewGuid()
			};

				dbContext.SessionQuestions.AddRange(
					new SessionQuestion { SessionId = testSessionId, QuestionId = questionIds[0] },
					new SessionQuestion { SessionId = testSessionId, QuestionId = questionIds[1] },
					new SessionQuestion { SessionId = testSessionId, QuestionId = questionIds[2] }
				);

				await dbContext.SaveChangesAsync();
			}

			return dbContext;
		}

		[Fact]
		public async Task SessionQuestionRepository_GetAssignedQuestions_ReturnAssignedQuestions()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
			var sessionQuestionRepository = new SessionQuestionRepository(dbContext);
			var testSessionId = dbContext.SessionQuestions.First().SessionId;
			var questionIds = dbContext.SessionQuestions.Select(q => q.QuestionId);

            // Act
            var assignedQuestions = await sessionQuestionRepository.GetAssignedQuestions(testSessionId);

            // Assert
            Assert.NotNull(assignedQuestions);
			Assert.Equal(questionIds.Count(), assignedQuestions.Count);
			Assert.All(questionIds, id => Assert.Contains(id, assignedQuestions));
		}
	}
}
