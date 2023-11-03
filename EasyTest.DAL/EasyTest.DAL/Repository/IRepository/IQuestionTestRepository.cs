using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IQuestionTestRepository<TKey> : IRepository<QuestionTest<TKey>, TKey> where TKey : IEquatable<TKey>
	{
		void Update(QuestionTest<TKey> question);
	}
}
