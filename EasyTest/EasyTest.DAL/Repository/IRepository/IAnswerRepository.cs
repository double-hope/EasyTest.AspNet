using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IAnswerRepository : IRepository<Answer>
	{
		public Task<Answer> GetById(Guid id);
		public Task<List<Answer>> GetByQuestionId(Guid questionId);
	}
}
