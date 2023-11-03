using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface ITestRepository : IRepository<Test>
	{
		void Update(Test test);
	}
}
