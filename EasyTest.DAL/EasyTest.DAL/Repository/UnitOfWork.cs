using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class UnitOfWork<TKey> : IUnitOfWork<TKey> where TKey : IEquatable<TKey>
	{
		private readonly ApplicationDbContext<TKey> _context;
		public IAnswerRepository<TKey> Answer { get; private set; }

		public IQuestionRepository<TKey> Question { get; private set; }

		public IQuestionTestRepository<TKey> QuestionTest { get; private set; }

		public ISessionAnswerRepository<TKey> SessionAnswer { get; private set; }

		public ISessionQuestionRepository<TKey> SessionQuestion { get; private set; }

		public ITestRepository<TKey> Test { get; private set; }

		public ITestSessionRepository<TKey> TestSession { get; private set; }

		public IUserRepository<TKey> User { get; private set; }
		public UnitOfWork(ApplicationDbContext<TKey> context)
		{
			_context = context;
			Answer = new AnswerRepository<TKey>(context);
			Question = new QuestionRepository<TKey>(context);
			QuestionTest = new QuestionTestRepository<TKey>(context);
			SessionAnswer = new SessionAnswerRepository<TKey>(context);
			SessionQuestion = new SessionQuestionRepository<TKey>(context);
			Test = new TestRepository<TKey>(context);
			TestSession = new TestSessionRepository<TKey>(context);
			User = new UserRepository<TKey>(context);
		}

		public void Save()
		{
			_context.SaveChanges();
		}
	}
}