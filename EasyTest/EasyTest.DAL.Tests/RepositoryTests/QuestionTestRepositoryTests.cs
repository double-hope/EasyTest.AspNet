using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.Tests.Repository
{
	[Trait("Category", "Unit")]
	public class QuestionTestRepositoryTests
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
				dbContext.Tests.Add(new Test
				{
					Title = "Test",
					Description = "Description"
				});
				dbContext.SaveChanges();
			}

			return dbContext;
		}

		[Fact]
		public async Task QuestionTestRepository_GetQuestionsByTestId_ReturnQuestions()
        {
            // Arrange
            var dbContext = await GetApplicationDbContext();
			var questionTestRepository = new QuestionTestRepository(dbContext);

			var testId = dbContext.Tests.First().Id;
			var questions = new List<Question>
			{
				new Question
				{
					Id = Guid.NewGuid(),
					Title = "SampleQuestion1",
					Text = "SampleText1",
				},
				new Question
				{
					Id = Guid.NewGuid(),
					Title = "SampleQuestion2",
					Text = "SampleText2",
				}
			};
			dbContext.Questions.AddRange(questions);

			dbContext.QuestionTests.AddRange(
				questions.Select(question => new QuestionTest
				{
					TestId = testId,
					QuestionId = question.Id,
				})
			);
			await dbContext.SaveChangesAsync();

            // Act
            var result = await questionTestRepository.GetQuestionsByTestId(testId);

            // Assert
            Assert.NotNull(result);
			Assert.Equal(questions.Count, result.Count);
			foreach (var expectedQuestion in questions)
			{
				Assert.Contains(result, actualQuestion => actualQuestion.Id == expectedQuestion.Id);
			}
		}
	}
}
