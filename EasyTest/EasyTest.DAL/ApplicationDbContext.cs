using EasyTest.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL
{
	public class ApplicationDbContext: IdentityDbContext<User, IdentityRole<Guid>, Guid>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<Answer> Answers { get; set; } 
		public DbSet<Question> Questions { get; set; } 
		public DbSet<QuestionTest> QuestionTests { get; set; } 
		public DbSet<SessionAnswer> SessionAnswers { get; set; } 
		public DbSet<SessionQuestion> SessionQuestions { get; set; } 
		public DbSet<Test> Tests { get; set; }
		public DbSet<TestSession> TestSessions { get; set; }
		public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Question>()
                .HasMany(e => e.Tests)
                .WithMany(e => e.Questions)
                .UsingEntity<QuestionTest>(
                    l => l.HasOne<Test>(e => e.Test).WithMany(e => e.QuestionTests),
                    r => r.HasOne<Question>(e => e.Question).WithMany(e => e.QuestionTests));

            builder.Entity<Question>()
                .HasMany(e => e.TestSessions)
                .WithMany(e => e.Questions)
                .UsingEntity<SessionQuestion>(
                    l => l.HasOne<TestSession>(e => e.Session).WithMany(e => e.SessionQuestions),
                    r => r.HasOne<Question>(e => e.Question).WithMany(e => e.SessionQuestions));

            builder.Entity<Answer>()
                .HasMany(e => e.Sessions)
                .WithMany(e => e.Answers)
                .UsingEntity<SessionAnswer>(
                    l => l.HasOne<TestSession>(e => e.Session).WithMany(e => e.SessionAnswers),
                    r => r.HasOne<Answer>(e => e.Answer).WithMany(e => e.SessionAnswers));
        }
    }
}