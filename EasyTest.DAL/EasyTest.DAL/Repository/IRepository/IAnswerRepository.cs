using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IAnswerRepository<TKey> : IRepository<Answer<TKey>, TKey> where TKey : IEquatable<TKey>
	{
		void Update(Answer<TKey> answer);
	}
}
