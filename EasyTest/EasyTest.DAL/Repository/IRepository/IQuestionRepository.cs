using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IQuestionRepository : IRepository<Question>
	{
		void Update(Question question);
	}
}
