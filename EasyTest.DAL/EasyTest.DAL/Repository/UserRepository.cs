using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		private readonly ApplicationDbContext _context;

		public UserRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public void Update(User user)
		{
			_context.Users.Update(user);
		}
	}
}
