using System.Linq.Expressions;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null);

        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter);
		Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
	}
}
