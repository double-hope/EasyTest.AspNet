using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class TestSessionRepository<TKey> : Repository<TestSession<TKey>, TKey>, ITestSessionRepository<TKey> where TKey : IEquatable<TKey>
	{
		private readonly ApplicationDbContext<TKey> _context;

		public TestSessionRepository(ApplicationDbContext<TKey> context) : base(context)
		{
			_context = context;
		}

		public void Update(TestSession<TKey> testSession)
		{
			_context.TestSessions.Update(testSession);
		}
	}
}
