using Microsoft.EntityFrameworkCore;

using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class UserTestRepository : Repository<UserTest>, IUserTestRepository
	{
		public UserTestRepository(ApplicationDbContext context) : base(context) { }

        public async Task<UserTest> GetByUserIdAndTestId(Guid userId, Guid testId)
        {
            IQueryable<UserTest> query = dbSet;

            query = query.Where(x => x.UserId.Equals(userId) && x.TestId.Equals(testId));

            return await query.FirstOrDefaultAsync();
        }
    }
}
