namespace EasyTest.DAL.Repository.IRepository
{
	public interface IUnitOfWork<TKey> where TKey : IEquatable<TKey>
	{
		IAnswerRepository<TKey> Answer { get; }
		IQuestionRepository<TKey> Question { get; }
		IQuestionTestRepository<TKey> QuestionTest { get; }
		ISessionAnswerRepository<TKey> SessionAnswer { get; }
		ISessionQuestionRepository<TKey> SessionQuestion { get; }
		ITestRepository<TKey> Test { get; }
		ITestSessionRepository<TKey> TestSession { get; }
		IUserRepository<TKey> User { get; }
		void Save();
	}
}
