﻿using EasyTest.DAL.Entities;

namespace EasyTest.DAL.Repository.IRepository
{
	public interface IQuestionRepository : IRepository<Question>
	{
		public Task<Question> GetById(Guid id);
	}
}
