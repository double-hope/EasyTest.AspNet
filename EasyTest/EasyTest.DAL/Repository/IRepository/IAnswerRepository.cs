using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IAnswerRepository : IRepository<Answer>
	{
		public Task<List<Answer>> GetByQuestionId(Guid questionId);
	}
}
