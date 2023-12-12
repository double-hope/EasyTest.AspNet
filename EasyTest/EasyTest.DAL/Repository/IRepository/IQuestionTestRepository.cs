using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IQuestionTestRepository : IRepository<QuestionTest>
	{
		public Task<List<Question>> GetQuestionsByTestId(Guid testId);
	}
}
