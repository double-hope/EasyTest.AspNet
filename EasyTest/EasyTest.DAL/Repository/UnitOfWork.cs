using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore.Storage;

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
		private ITestRepository testRepository;
		private IRepository<TestSession> testSessionRepository;
		private IUserRepository userRepository;
		private IRepository<UserTest> userTestRepository;
		private IDbContextTransaction _transaction;
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
		public ITestRepository TestRepository
		{
			get
			{
				if (testRepository == null)
				{
					testRepository = new TestRepository(_context);
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
		public IUserRepository UserRepository
		{
			get
			{
				if (userRepository == null)
				{
					userRepository = new UserRepository(_context);
				}
				return userRepository;
			}
		}
		public IRepository<UserTest> UserTestRepository
		{
			get
			{
				if (userTestRepository == null)
				{
					userTestRepository = new Repository<UserTest>(_context);
				}
				return userTestRepository;
			}
		}

		public async Task<IDbContextTransaction> BeginTransaction()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
			return _transaction;
        }

        public async Task Commit()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await Rollback().ConfigureAwait(false);
                throw;
            }
        }

        public async Task Rollback()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
        public async Task Save()
		{
			await _context.SaveChangesAsync();
		}
	}
}