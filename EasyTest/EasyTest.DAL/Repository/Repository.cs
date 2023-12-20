﻿using EasyTest.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		protected readonly ApplicationDbContext _context;
		internal DbSet<T> dbSet;

		public Repository(ApplicationDbContext context)
		{
			_context = context;
			dbSet = _context.Set<T>();
		}

		public async Task<IEnumerable<T>> GetAll()
		{
			IQueryable<T> query = dbSet;

			return await query.ToListAsync();
		}

		public async Task<T> GetFirstOrDefault()
		{
			IQueryable<T> query = dbSet;

            return await query.FirstOrDefaultAsync();
		}
		public async Task Add(T entity)
		{
			await dbSet.AddAsync(entity);
			await _context.SaveChangesAsync();
		}
        public async Task AddRange(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
			await _context.SaveChangesAsync();
		}

        public void Update(T entity)
		{
			dbSet.Update(entity);
			_context.SaveChanges();
		}

		public void Remove(T entity)
		{
			dbSet.Remove(entity);
			_context.SaveChanges();
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			dbSet.RemoveRange(entities);
			_context.SaveChanges();
		}
	}
}
