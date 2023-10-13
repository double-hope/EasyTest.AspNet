using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IUserRepository : IRepository<User>
	{
		void Update(User user);
	}
}
