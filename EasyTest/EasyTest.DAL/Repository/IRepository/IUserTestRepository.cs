using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IUserTestRepository : IRepository<UserTest>
	{
		public Task<UserTest> GetByUserIdAndTestId(Guid userId, Guid testId);
    }
}
