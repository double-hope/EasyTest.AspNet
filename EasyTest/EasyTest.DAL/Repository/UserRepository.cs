using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.Repository
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		public UserRepository(ApplicationDbContext context) : base(context) { }

		public async Task<User> GetByEmail(string email)
		{
			IQueryable<User> query = dbSet;

			query = query.Where(x => x.Email.Equals(email));

			return await query.FirstOrDefaultAsync();
		}
	}
}
