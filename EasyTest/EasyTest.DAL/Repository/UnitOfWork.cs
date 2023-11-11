using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		public IAnswerRepository Answer { get; private set; }

		public IQuestionRepository Question { get; private set; }

		public IQuestionTestRepository QuestionTest { get; private set; }

		public ISessionAnswerRepository SessionAnswer { get; private set; }

		public ISessionQuestionRepository SessionQuestion { get; private set; }

		public ITestRepository Test { get; private set; }

		public ITestSessionRepository TestSession { get; private set; }

		public IUserRepository User { get; private set; }
		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
			Answer = new AnswerRepository(context);
			Question = new QuestionRepository(context);
			QuestionTest = new QuestionTestRepository(context);
			SessionAnswer = new SessionAnswerRepository(context);
			SessionQuestion = new SessionQuestionRepository(context);
			Test = new TestRepository(context);
			TestSession = new TestSessionRepository(context);
			User = new UserRepository(context);
		}

		public void Save()
		{
			_context.SaveChanges();
		}
	}
}