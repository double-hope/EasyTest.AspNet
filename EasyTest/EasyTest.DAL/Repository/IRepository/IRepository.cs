using System.Linq.Expressions;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAll();
        Task<T> GetFirstOrDefault();
		Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
	}
}
