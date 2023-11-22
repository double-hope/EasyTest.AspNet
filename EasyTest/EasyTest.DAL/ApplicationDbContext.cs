using EasyTest.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL
{
	public class ApplicationDbContext: IdentityDbContext<IdentityUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<Answer> Answers {  get; set; } 
		public DbSet<Question> Questions {  get; set; } 
		public DbSet<QuestionTest> QuestionTests {  get; set; } 
		public DbSet<SessionAnswer> SessionAnswers {  get; set; } 
		public DbSet<SessionQuestion> SessionQuestions {  get; set; } 
		public DbSet<Test> Tests {  get; set; }
		public DbSet<TestSession> TestSessions {  get; set; }
		public DbSet<User> Users {  get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}