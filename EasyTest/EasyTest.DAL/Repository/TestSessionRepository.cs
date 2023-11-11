using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class TestSessionRepository : Repository<TestSession>, ITestSessionRepository
	{
		private readonly ApplicationDbContext _context;

		public TestSessionRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public void Update(TestSession testSession)
		{
			_context.TestSessions.Update(testSession);
		}
	}
}
