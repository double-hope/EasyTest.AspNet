using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface ISessionQuestionRepository<TKey> : IRepository<SessionQuestion<TKey>, TKey> where TKey : IEquatable<TKey>
	{
		void Update(SessionQuestion<TKey> sessionQuestion);
	}
}
