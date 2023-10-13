using System.Linq.Expressions;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null);
	}
}
