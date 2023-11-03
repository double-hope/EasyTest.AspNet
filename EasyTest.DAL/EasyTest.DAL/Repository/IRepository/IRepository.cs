using System.Linq.Expressions;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IRepository<TEntity, TKey> 
		where TEntity : class 
		where TKey : IEquatable<TKey> 
	{
		IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null);
		TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter);
		void Add(TEntity entity);
		void Remove(TEntity entity);
		void RemoveRange(IEnumerable<TEntity> entities);
	}
}
