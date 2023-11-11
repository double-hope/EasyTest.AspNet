using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface ITestSessionRepository : IRepository<TestSession>
	{
		void Update(TestSession testSession);
	}
}
