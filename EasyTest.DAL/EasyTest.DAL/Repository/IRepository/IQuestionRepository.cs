using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IQuestionRepository<TKey> : IRepository<Question<TKey>, TKey> where TKey : IEquatable<TKey>
	{
		void Update(Question<TKey> question);
	}
}
