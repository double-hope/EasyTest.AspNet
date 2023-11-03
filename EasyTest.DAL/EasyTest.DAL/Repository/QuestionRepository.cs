using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class QuestionRepository<TKey> : Repository<Question<TKey>, TKey>, IQuestionRepository<TKey> where TKey : IEquatable<TKey>
	{
		private readonly ApplicationDbContext<TKey> _context;
		public QuestionRepository(ApplicationDbContext<TKey> context) : base(context)
		{
			_context = context;
		}

		public void Update(Question<TKey> question)
		{
			_context.Questions.Update(question);
		}
	}
}
