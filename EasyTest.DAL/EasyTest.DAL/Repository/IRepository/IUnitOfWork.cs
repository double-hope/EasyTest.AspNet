namespace EasyTest.DAL.Repository.IRepository
{
	public interface IUnitOfWork
	{
		IAnswerRepository Answer { get; }
		IQuestionRepository Question { get; }
		IQuestionTestRepository QuestionTest { get; }
		ISessionAnswerRepository SessionAnswer { get; }
		ISessionQuestionRepository SessionQuestion { get; }
		ITestRepository Test { get; }
		ITestSessionRepository TestSession { get; }
		IUserRepository User { get; }
		void Save();
	}
}
