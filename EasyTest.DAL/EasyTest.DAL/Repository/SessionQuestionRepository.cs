using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class SessionQuestionRepository<TKey> : Repository<SessionQuestion<TKey>, TKey>, ISessionQuestionRepository<TKey> where TKey : IEquatable<TKey>
	{
		private readonly ApplicationDbContext<TKey> _context;
		public SessionQuestionRepository(ApplicationDbContext<TKey> context) : base(context)
		{
			_context = context;
		}

		public void Update(SessionQuestion<TKey> sessionQuestion)
		{
			_context.SessionQuestions.Update(sessionQuestion);
		}
	}
}
