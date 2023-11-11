using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class AnswerRepository : Repository<Answer>, IAnswerRepository
	{
		public readonly ApplicationDbContext _context;
		public AnswerRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public void Update(Answer answer)
		{
			_context.Answers.Update(answer);
		}
	}
}
