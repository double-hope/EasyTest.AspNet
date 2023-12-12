using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface ISessionQuestionRepository : IRepository<SessionQuestion>
	{

		public Task<List<Guid>> GetAssignedQuestions(Guid sessionId);
	}
}
