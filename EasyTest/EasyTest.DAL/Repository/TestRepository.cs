using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.Repository
{
	public class TestRepository : Repository<Test>, ITestRepository
	{
		public TestRepository(ApplicationDbContext context) : base(context) { }

		public async Task<Test> GetById(Guid id)
		{
			IQueryable<Test> query = dbSet.Include(x => x.Questions);
			query = query.Where(x => x.Id == id);

			return await query.FirstOrDefaultAsync();
		}
    }
}
