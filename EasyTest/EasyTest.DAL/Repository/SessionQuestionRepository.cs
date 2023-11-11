using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class SessionQuestionRepository : Repository<SessionQuestion>, ISessionQuestionRepository
	{
		private readonly ApplicationDbContext _context;
		public SessionQuestionRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public void Update(SessionQuestion sessionQuestion)
		{
			_context.SessionQuestions.Update(sessionQuestion);
		}
	}
}
