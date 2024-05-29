using Microsoft.EntityFrameworkCore.Storage;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IUnitOfWork
	{
		IAnswerRepository AnswerRepository { get; }
		IQuestionRepository QuestionRepository { get; }
		IQuestionTestRepository QuestionTestRepository { get; }
		ISessionAnswerRepository SessionAnswerRepository { get; }
		ISessionQuestionRepository SessionQuestionRepository { get; }
		ITestRepository TestRepository { get; }
		ITestSessionRepository TestSessionRepository { get; }
		IUserRepository UserRepository { get; }
		IUserTestRepository UserTestRepository { get; }
		Task<IDbContextTransaction> BeginTransaction();
		Task Commit();
		Task Rollback();
        Task Save();
	}
}
