using EasyTest.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL
{
	public class ApplicationDbContext<TKey>: IdentityDbContext<User<TKey>, IdentityRole<TKey>, TKey> where TKey : IEquatable<TKey>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext<TKey>> options) : base(options) { }

		public DbSet<Answer<TKey>> Answers {  get; set; } 
		public DbSet<Question<TKey>> Questions {  get; set; } 
		public DbSet<QuestionTest<TKey>> QuestionTests {  get; set; } 
		public DbSet<SessionAnswer<TKey>> SessionAnswers {  get; set; } 
		public DbSet<SessionQuestion<TKey>> SessionQuestions {  get; set; } 
		public DbSet<Test<TKey>> Tests {  get; set; }
		public DbSet<TestSession<TKey>> TestSessions {  get; set; }
		public DbSet<User<TKey>> Users {  get; set; }
	}
}