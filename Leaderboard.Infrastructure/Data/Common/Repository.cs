using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Infrastructure.Data.Common
{
	public class Repository : IRepository
	{
		private readonly DbContext context;

		public Repository(LeaderboardDbContext context)
		{
			this.context = context;
		}

		private DbSet<T> DbSet<T>() where T : class
		{
			return context.Set<T>();
		}

		public IQueryable<T> All<T>() where T : class
		{
			return DbSet<T>();
		}

		public IQueryable<T> AllAsReadOnly<T>() where T : class
		{
			return DbSet<T>().AsNoTracking();
		}
	}
}
