using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface ITestSessionRepository<TKey> : IRepository<TestSession<TKey>, TKey> where TKey : IEquatable<TKey>
	{
		void Update(TestSession<TKey> testSession);
	}
}
