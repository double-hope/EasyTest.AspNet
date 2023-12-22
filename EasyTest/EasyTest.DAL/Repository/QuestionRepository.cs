using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.Repository
{
	public class QuestionRepository : Repository<Question>, IQuestionRepository
	{
		public QuestionRepository(ApplicationDbContext context) : base(context) { }

		public async Task<Question> GetById(Guid id)
		{
			IQueryable<Question> query = dbSet;
			query = query.Where(x => x.Id == id);

			return await query.FirstOrDefaultAsync();
		}
	}
}
