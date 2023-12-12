using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface ISessionAnswerRepository : IRepository<SessionAnswer>
	{
		public Task<List<SessionAnswer>> GetCorrectAnswers(Guid sessionId);
	}
}
