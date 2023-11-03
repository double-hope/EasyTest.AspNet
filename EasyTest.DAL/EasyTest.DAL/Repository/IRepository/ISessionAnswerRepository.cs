using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface ISessionAnswerRepository<TKey> : IRepository<SessionAnswer<TKey>, TKey> where TKey : IEquatable<TKey>
	{
		void Update(SessionAnswer<TKey> sessionAnswer);
	}
}
