using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class SessionAnswerRepository<TKey> : Repository<SessionAnswer<TKey>, TKey>, ISessionAnswerRepository<TKey> where TKey : IEquatable<TKey>
	{
		private readonly ApplicationDbContext<TKey> _context;
		public SessionAnswerRepository(ApplicationDbContext<TKey> context) : base(context)
		{
			_context = context;
		}

		public void Update(SessionAnswer<TKey> sessionAnswer)
		{
			_context.SessionAnswers.Update(sessionAnswer);
		}
	}
}
