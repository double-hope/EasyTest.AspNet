using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class UserRepository<TKey> : Repository<User<TKey>, TKey>, IUserRepository<TKey> where TKey : IEquatable<TKey>
	{
		private readonly ApplicationDbContext<TKey> _context;

		public UserRepository(ApplicationDbContext<TKey> context) : base(context)
		{
			_context = context;
		}

		public void Update(User<TKey> user)
		{
			_context.Users.Update(user);
		}
	}
}
