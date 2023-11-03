using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class QuestionTestRepository<TKey> : Repository<QuestionTest<TKey>, TKey>, IQuestionTestRepository<TKey> where TKey : IEquatable<TKey>
	{
		private readonly ApplicationDbContext<TKey> _context;
		public QuestionTestRepository(ApplicationDbContext<TKey> context) : base(context)
		{
			_context = context;
		}

		public void Update(QuestionTest<TKey> question)
		{
			_context.QuestionTests.Update(question);
		}
	}
}
