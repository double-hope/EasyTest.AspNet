using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface ITestRepository : IRepository<Test>
	{
		public Task<Test> GetById(Guid id);
	}
}
