using EasyTest.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EasyTest.DAL.Repository
{
	public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> 
		where TEntity : class
		where TKey : IEquatable<TKey>
	{
		private readonly ApplicationDbContext<TKey> _context;
		internal DbSet<TEntity> dbSet;

		public Repository(ApplicationDbContext<TKey> context)
		{
			_context = context;
			dbSet = _context.Set<TEntity>();
		}

		public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null)
		{
			IQueryable<TEntity> query = dbSet;
			if (filter != null) query = query.Where(filter);

			return query.ToList();
		}

		public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter)
		{
			IQueryable<TEntity> query = dbSet;
			query = query.Where(filter);
			
			return query.FirstOrDefault();
		}
		public void Add(TEntity entity)
		{
			dbSet.Add(entity);
		}

		public void Remove(TEntity entity)
		{
			dbSet.Remove(entity);
		}

		public void RemoveRange(IEnumerable<TEntity> entities)
		{
			dbSet.RemoveRange(entities);
		}
	}
}
