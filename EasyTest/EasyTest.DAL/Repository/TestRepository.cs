﻿using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace EasyTest.DAL.Repository
{
	public class TestRepository : Repository<Test>, ITestRepository
	{
		public TestRepository(ApplicationDbContext context) : base(context) { }

		public async Task<Test> GetById(Guid id)
		{
			IQueryable<Test> query = dbSet;
			dbSet.Where(x => x.Id == id);

			return await query.FirstOrDefaultAsync();
		}
	}
}