using EasyTest.DAL.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IUnitOfWork
	{
		IRepository<Answer> AnswerRepository { get; }
		IRepository<Question> QuestionRepository { get; }
		IRepository<QuestionTest> QuestionTestRepository { get; }
		IRepository<SessionAnswer> SessionAnswerRepository { get; }
		IRepository<SessionQuestion> SessionQuestionRepository { get; }
		IRepository<Test> TestRepository { get; }
		IRepository<TestSession> TestSessionRepository { get; }
		IRepository<User> UserRepository { get; }
		Task<IDbContextTransaction> BeginTransaction();
		Task Commit();
		Task Rollback();
        Task Save();
	}
}
