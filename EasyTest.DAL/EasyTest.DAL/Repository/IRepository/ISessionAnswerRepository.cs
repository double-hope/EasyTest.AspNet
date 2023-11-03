using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface ISessionAnswerRepository : IRepository<SessionAnswer>
	{
		void Update(SessionAnswer sessionAnswer);
	}
}
