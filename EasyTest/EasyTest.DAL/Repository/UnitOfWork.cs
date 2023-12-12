using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore.Storage;

namespace EasyTest.DAL.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;

		private IAnswerRepository answerRepository;
		private IRepository<Question> questionRepository;
		private IQuestionTestRepository questionTestRepository;
		private ISessionAnswerRepository sessionAnswerRepository;
		private ISessionQuestionRepository sessionQuestionRepository;
		private ITestRepository testRepository;
		private ITestSessionRepository testSessionRepository;
		private IUserRepository userRepository;
		private IRepository<UserTest> userTestRepository;
		private IDbContextTransaction _transaction;
        public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
		}
		public IAnswerRepository AnswerRepository
		{
			get
			{
				if (answerRepository == null)
				{
					answerRepository = new AnswerRepository(_context);
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
		public IQuestionTestRepository QuestionTestRepository
		{
			get
			{
				if (questionTestRepository == null)
				{
					questionTestRepository = new QuestionTestRepository(_context);
				}
				return questionTestRepository;
			}
		}
		public ISessionAnswerRepository SessionAnswerRepository
		{
			get
			{
				if (sessionAnswerRepository == null)
				{
					sessionAnswerRepository = new SessionAnswerRepository(_context);
				}
				return sessionAnswerRepository;
			}
		}
		public ISessionQuestionRepository SessionQuestionRepository
		{
			get
			{
				if (sessionQuestionRepository == null)
				{
					sessionQuestionRepository = new SessionQuestionRepository(_context);
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
		public ITestSessionRepository TestSessionRepository
		{
			get
			{
				if (testSessionRepository == null)
				{
					testSessionRepository = new TestSessionRepository(_context);
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