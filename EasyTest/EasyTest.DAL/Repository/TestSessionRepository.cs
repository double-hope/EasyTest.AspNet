using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using EasyTest.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.Repository
{
	public class TestSessionRepository : Repository<TestSession>, ITestSessionRepository
	{
		public TestSessionRepository(ApplicationDbContext context) : base(context) { }

		public async Task<TestSession> GetInProgressSession(Guid userId, Guid testId)
		{
			IQueryable<TestSession> query = dbSet;

			query = query.Where(x => x.UserId == userId && x.TestId == testId && x.Status == TestStatus.InProgress);

			return await query.FirstOrDefaultAsync();
		}

		public async Task<TestSession> GetSession(Guid sessionId)
		{
			IQueryable<TestSession> query = dbSet;

			query = query
				.Include(ts => ts.Test)
				.Include(ts => ts.Questions)
				.Where(x => x.Id == sessionId);

			return await query.FirstOrDefaultAsync();
		}
	}
}
