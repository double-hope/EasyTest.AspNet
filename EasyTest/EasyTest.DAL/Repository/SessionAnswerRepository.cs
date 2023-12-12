using EasyTest.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.Repository.IRepository
{
	public class SessionAnswerRepository : Repository<SessionAnswer>, ISessionAnswerRepository
	{
		public SessionAnswerRepository(ApplicationDbContext context) : base(context) { }

		public async Task<List<SessionAnswer>> GetCorrectAnswers(Guid sessionId)
		{
			IQueryable<SessionAnswer> query = dbSet;

			query = query
				.Include(ts => ts.Answer)
				.Where(x => x.SessionId == sessionId && x.IsCorrect);

			return await query.ToListAsync();
		}
	}
}
