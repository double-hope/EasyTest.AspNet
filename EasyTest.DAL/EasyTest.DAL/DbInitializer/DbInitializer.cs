using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.DbInitializer
{
	public class DbInitializer<TKey> : IDbInitializer where TKey : IEquatable<TKey>
	{
		private readonly ApplicationDbContext<TKey> _context;

		public DbInitializer(ApplicationDbContext<TKey> context)
		{
			_context = context;
		}

		public void Initialize()
		{
			try
			{
				if (_context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Count() > 0)
				{
					_context.Database.Migrate();
				}
			}
			catch (Exception)
			{
				throw;
			}

			return;
		}
	}
}
