using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface ITestRepository<TKey> : IRepository<Test<TKey>, TKey> where TKey : IEquatable<TKey>
	{
		void Update(Test<TKey> test);
	}
}
