using EasyTest.DAL.Repository.IRepository;
using System.Linq.Expressions;

namespace EasyTest.DAL.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null)
		{
			throw new NotImplementedException();
		}
	}
}
