using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IUserRepository : IRepository<User>
	{
		public Task<User> GetByEmail(string email);
	}
}
