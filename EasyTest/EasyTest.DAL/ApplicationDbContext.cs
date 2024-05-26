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
		public DbSet<UserTest> UserTests { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Question>()
                .HasMany(e => e.Tests)
                .WithMany(e => e.Questions)
                .UsingEntity<QuestionTest>(
                    l => l.HasOne(e => e.Test).WithMany(e => e.QuestionTests),
                    r => r.HasOne(e => e.Question).WithMany(e => e.QuestionTests));

            builder.Entity<Question>()
                .HasMany(e => e.TestSessions)
                .WithMany(e => e.Questions)
                .UsingEntity<SessionQuestion>(
                    l => l.HasOne(e => e.Session).WithMany(e => e.SessionQuestions),
                    r => r.HasOne(e => e.Question).WithMany(e => e.SessionQuestions));

            builder.Entity<Answer>()
                .HasMany(e => e.Sessions)
                .WithMany(e => e.Answers)
                .UsingEntity<SessionAnswer>(
                    l => l.HasOne(e => e.Session).WithMany(e => e.SessionAnswers),
                    r => r.HasOne(e => e.Answer).WithMany(e => e.SessionAnswers));

            builder.Entity<Test>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Tests)
                .UsingEntity<UserTest>(
                    l => l.HasOne(e => e.User).WithMany(e => e.UserTests),
                    r => r.HasOne(e => e.Test).WithMany(e => e.UserTests));
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity<Guid>>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        entry.Entity.UpdatedAt = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}