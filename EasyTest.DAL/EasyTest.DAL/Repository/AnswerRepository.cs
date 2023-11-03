using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class AnswerRepository<TKey> : Repository<Answer<TKey>, TKey>, IAnswerRepository<TKey> where TKey : IEquatable<TKey>
	{
		public readonly ApplicationDbContext<TKey> _context;
		public AnswerRepository(ApplicationDbContext<TKey> context) : base(context)
		{
			_context = context;
		}

		public void Update(Answer<TKey> answer)
		{
			_context.Answers.Update(answer);
		}
	}
}
