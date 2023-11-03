using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class QuestionRepository : Repository<Question>, IQuestionRepository
	{
		private readonly ApplicationDbContext _context;
		public QuestionRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public void Update(Question question)
		{
			_context.Questions.Update(question);
		}
	}
}
