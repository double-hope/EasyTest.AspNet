using EasyTest.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EasyTest.DAL.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context;
		internal DbSet<T> dbSet;

		public Repository(ApplicationDbContext context)
		{
			_context = context;
			dbSet = _context.Set<T>();
		}

		public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null)
		{
			IQueryable<T> query = dbSet;
			if (filter != null) query = query.Where(filter);

			return await query.ToListAsync();
		}

		public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter)
		{
			IQueryable<T> query = dbSet;
			query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
		}
		public async Task Add(T entity)
		{
			await dbSet.AddAsync(entity);
		}

		public void Update(T entity)
		{
			dbSet.Update(entity);
		}

		public void Remove(T entity)
		{
			dbSet.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			dbSet.RemoveRange(entities);
		}
	}
}
