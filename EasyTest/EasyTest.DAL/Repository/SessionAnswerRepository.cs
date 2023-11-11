using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class SessionAnswerRepository : Repository<SessionAnswer>, ISessionAnswerRepository
	{
		private readonly ApplicationDbContext _context;
		public SessionAnswerRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public void Update(SessionAnswer sessionAnswer)
		{
			_context.SessionAnswers.Update(sessionAnswer);
		}
	}
}
