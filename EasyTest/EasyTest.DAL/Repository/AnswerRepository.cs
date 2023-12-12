using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.Repository
{
	public class AnswerRepository : Repository<Answer>, IAnswerRepository
	{
		public AnswerRepository(ApplicationDbContext context) : base(context) { }

		public async Task<Answer> GetById(Guid id)
		{
			IQueryable<Answer> query = dbSet;

			var answer = await query
				.Where(a => a.Id == id)
				.FirstOrDefaultAsync();

			return answer;
		}

		public async Task<List<Answer>> GetByQuestionId(Guid questionId)
		{
			IQueryable<Answer> query = dbSet;

			var answers = await query
				.Where(a => a.QuestionId == questionId)
				.ToListAsync();

			return answers;
		}
	}
}
