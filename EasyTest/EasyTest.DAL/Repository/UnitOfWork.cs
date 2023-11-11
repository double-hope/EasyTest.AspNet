using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;

		private IRepository<Answer> answerRepository;
		private IRepository<Question> questionRepository;
		private IRepository<QuestionTest> questionTestRepository;
		private IRepository<SessionAnswer> sessionAnswerRepository;
		private IRepository<SessionQuestion> sessionQuestionRepository;
		private IRepository<Test> testRepository;
		private IRepository<TestSession> testSessionRepository;
		private IRepository<User> userRepository;
		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
		}
		public IRepository<Answer> AnswerRepository
		{
			get
			{
				if (answerRepository == null)
				{
					answerRepository = new Repository<Answer>(_context);
				}
				return answerRepository;
			}
		}
		public IRepository<Question> QuestionRepository
		{
			get
			{
				if (questionRepository == null)
				{
					questionRepository = new Repository<Question>(_context);
				}
				return questionRepository;
			}
		}
		public IRepository<QuestionTest> QuestionTestRepository
		{
			get
			{
				if (questionTestRepository == null)
				{
					questionTestRepository = new Repository<QuestionTest>(_context);
				}
				return questionTestRepository;
			}
		}
		public IRepository<SessionAnswer> SessionAnswerRepository
		{
			get
			{
				if (sessionAnswerRepository == null)
				{
					sessionAnswerRepository = new Repository<SessionAnswer>(_context);
				}
				return sessionAnswerRepository;
			}
		}
		public IRepository<SessionQuestion> SessionQuestionRepository
		{
			get
			{
				if (sessionQuestionRepository == null)
				{
					sessionQuestionRepository = new Repository<SessionQuestion>(_context);
				}
				return sessionQuestionRepository;
			}
		}
		public IRepository<Test> TestRepository
		{
			get
			{
				if (testRepository == null)
				{
					testRepository = new Repository<Test>(_context);
				}
				return testRepository;
			}
		}
		public IRepository<TestSession> TestSessionRepository
		{
			get
			{
				if (testSessionRepository == null)
				{
					testSessionRepository = new Repository<TestSession>(_context);
				}
				return testSessionRepository;
			}
		}
		public IRepository<User> UserRepository
		{
			get
			{
				if (userRepository == null)
				{
					userRepository = new Repository<User>(_context);
				}
				return userRepository;
			}
		}

		public void Save()
		{
			_context.SaveChanges();
		}
	}
}