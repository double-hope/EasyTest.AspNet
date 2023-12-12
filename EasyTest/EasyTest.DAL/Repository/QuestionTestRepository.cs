using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.Repository
{
	public class QuestionTestRepository : Repository<QuestionTest>, IQuestionTestRepository
	{
		public QuestionTestRepository(ApplicationDbContext context) : base(context) { }

		public async Task<List<Question>> GetQuestionsByTestId(Guid testId)
		{
			IQueryable<QuestionTest> query = dbSet;
			var questions = await query
				.Where(qt => qt.TestId.Equals(testId))
				.Select(qt => qt.Question)
				.ToListAsync();

			return questions;
		}
	}
}
