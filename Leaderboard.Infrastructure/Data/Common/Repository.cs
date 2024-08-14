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

		public async Task AddAsync<T>(T entity) where T : class
		{
			await DbSet<T>().AddAsync(entity);
		}

		public IQueryable<T> All<T>() where T : class
		{
			return DbSet<T>();
		}

		public IQueryable<T> AllAsReadOnly<T>() where T : class
		{
			return DbSet<T>().AsNoTracking();
		}

		public async Task<T?> GetByIdAsync<T>(object id) where T : class
		{
			return await DbSet<T>().FindAsync(id);
		}

		public async Task DeleteAsync<T>(object id) where T : class
		{
			T? entity = await GetByIdAsync<T>(id);

			if (entity != null)
			{
				DbSet<T>().Remove(entity);
			}
		}

		public void Delete<T>(T entity) where T : class
		{
			DbSet<T>().Remove(entity);
		}

		public void DeleteRange<T>(IEnumerable<T> entities) where T : class
		{
			DbSet<T>().RemoveRange(entities);
		}

		public async Task<int> SaveChangesAsync()
		{
			return await context.SaveChangesAsync();
		}
	}
}
