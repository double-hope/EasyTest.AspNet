using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.DAL.Repository
{
	public class QuestionTestRepository : Repository<QuestionTest>, IQuestionTestRepository
	{
		private readonly ApplicationDbContext _context;
		public QuestionTestRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public void Update(QuestionTest question)
		{
			_context.QuestionTests.Update(question);
		}
	}
}
