using Microsoft.EntityFrameworkCore;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository;

namespace EasyTest.DAL.Tests.Repository
{
	[Trait("Category", "Unit")]
	public class AnswerRepositoryTests
	{
		private async Task<ApplicationDbContext> GetApplicationDbContext()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;
			var dbContext = new ApplicationDbContext(options);
			dbContext.Database.EnsureCreated();

			if (await dbContext.Questions.CountAsync() <= 0)
			{
				dbContext.Questions.Add(
						new Question()
						{
							Title = "Title",
							Text = "Text",
						}
					);
				await dbContext.SaveChangesAsync();
			}

			return dbContext;
		}

		[Fact]
		public async Task AnswerRepository_GetById_ReturnAnswer()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
			var answerRepository = new AnswerRepository(dbContext);

			var answerId = Guid.NewGuid();
			var answer = new Answer
			{
				Id = answerId,
				QuestionId = dbContext.Questions.First().Id,
				Text = "SampleAnswer",
				IsCorrect = true,
			};

			dbContext.Answers.Add(answer);
			await dbContext.SaveChangesAsync();

            // Act
            var result = await answerRepository.GetById(answerId);

            // Assert
            Assert.NotNull(result);
			Assert.Equal(answerId, result.Id);
		}

		[Fact]
		public async Task AnswerRepository_GetByQuestionId_ReturnAnswers()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
			var answerRepository = new AnswerRepository(dbContext);

			var questionId = dbContext.Questions.First().Id;
			var answers = new List<Answer>
			{
				new Answer
				{
					Id = Guid.NewGuid(),
					QuestionId = questionId,
					Text = "SampleAnswer1",
					IsCorrect = true,
				},
				new Answer
				{
					Id = Guid.NewGuid(),
					QuestionId = questionId,
					Text = "SampleAnswer2",
					IsCorrect = false,
				}
            };

			dbContext.Answers.AddRange(answers);
			await dbContext.SaveChangesAsync();

            // Act
            var result = await answerRepository.GetByQuestionId(questionId);

            // Assert
            Assert.NotNull(result);
			Assert.Equal(answers.Count, result.Count);
			foreach (var expectedAnswer in answers)
			{
				Assert.Contains(result, actualAnswer => actualAnswer.Id == expectedAnswer.Id);
			}
		}
	}
}
