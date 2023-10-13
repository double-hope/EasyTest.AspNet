using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IQuestionTestRepository : IRepository<QuestionTest>
	{
		void Update(QuestionTest question);
	}
}
