using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class TestRepository<TKey> : Repository<Test<TKey>, TKey>, ITestRepository<TKey> where TKey : IEquatable<TKey>
	{
		private readonly ApplicationDbContext<TKey> _context;
		public TestRepository(ApplicationDbContext<TKey> context) : base(context)
		{
			_context = context;
		}

		public void Update(Test<TKey> test)
		{
			_context.Tests.Update(test);
		}
	}
}
