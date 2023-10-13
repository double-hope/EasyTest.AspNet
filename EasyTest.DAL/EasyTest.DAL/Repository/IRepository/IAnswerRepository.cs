using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IAnswerRepository : IRepository<Answer>
	{
		void Update(Answer answer);
	}
}
