using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository;
using EasyTest.DAL.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.Tests.Repository
{
	public class SessionAnswerRepositoryTests
	{
		private async Task<ApplicationDbContext> GetApplicationDbContext()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;
			var dbContext = new ApplicationDbContext(options);
			dbContext.Database.EnsureCreated();

			if (dbContext.SessionAnswers.Count() <= 0)
			{
				var question = new Question { Id = Guid.NewGuid(), Title = "Question Title", Text = "Question Text" };

				var answer1 = new Answer { Id = Guid.NewGuid(), Text = "Answer 1", IsCorrect = true, QuestionId = question.Id };
				var answer2 = new Answer { Id = Guid.NewGuid(), Text = "Answer 2", IsCorrect = false, QuestionId = question.Id };
				var answer3 = new Answer { Id = Guid.NewGuid(), Text = "Answer 3", IsCorrect = true, QuestionId = question.Id };

				var sessionId = Guid.NewGuid();

				var sessionAnswer1 = new SessionAnswer { SessionId = sessionId, AnswerId = answer1.Id, IsCorrect = true };
				var sessionAnswer2 = new SessionAnswer { SessionId = sessionId, AnswerId = answer2.Id, IsCorrect = false };
				var sessionAnswer3 = new SessionAnswer { SessionId = sessionId, AnswerId = answer3.Id, IsCorrect = true };

				dbContext.Answers.AddRange(answer1, answer2, answer3);
				dbContext.SessionAnswers.AddRange(sessionAnswer1, sessionAnswer2, sessionAnswer3);

				await dbContext.SaveChangesAsync();
			}


			return dbContext;
		}

		[Fact]
		public async void SessionAnswerRepository_GetCorrectAnswers_ReturnCorrectAnswers()
		{
			var dbContext = await GetApplicationDbContext();
			var sessionAnswerRepository = new SessionAnswerRepository(dbContext);
			var sessionId = dbContext.SessionAnswers.First().SessionId;

			var result = await sessionAnswerRepository.GetCorrectAnswers(sessionId);

			Assert.NotNull(result);
			Assert.Equal(2, result.Count);
		}
	}
}
