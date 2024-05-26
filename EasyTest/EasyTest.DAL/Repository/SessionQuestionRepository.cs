using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.Repository
{
    public class SessionQuestionRepository : Repository<SessionQuestion>, ISessionQuestionRepository
	{
		public SessionQuestionRepository(ApplicationDbContext context) : base(context) { }

		public async Task<List<Guid>> GetAssignedQuestions(Guid sessionId)
		{
			IQueryable<SessionQuestion> query = dbSet;

			var questions = await query
				.Where(sq => sq.SessionId == sessionId)
				.Select(qt => qt.QuestionId)
				.ToListAsync();

			return questions;
		}

        public async Task<SessionQuestion> GetByQuestionIdAndSessionId(Guid questionId, Guid sessionId)
        {
            IQueryable<SessionQuestion> query = dbSet;

            query = query.Where(sq => sq.QuestionId == questionId && sq.SessionId == sessionId);

            return await query.FirstOrDefaultAsync();
        }
    }
}
