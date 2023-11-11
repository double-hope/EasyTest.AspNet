using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface ISessionQuestionRepository : IRepository<SessionQuestion>
	{
		void Update(SessionQuestion sessionQuestion);
	}
}
