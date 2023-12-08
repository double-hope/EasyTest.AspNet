using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface ITestSessionRepository : IRepository<TestSession>
	{
		public Task<TestSession> GetInProgressSession(Guid userId, Guid testId);
		public Task<TestSession> GetSession(Guid sessionId);
	}
}
