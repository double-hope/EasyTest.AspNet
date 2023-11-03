using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class TestRepository : Repository<Test>, ITestRepository
	{
		private readonly ApplicationDbContext _context;
		public TestRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public void Update(Test test)
		{
			_context.Tests.Update(test);
		}
	}
}
