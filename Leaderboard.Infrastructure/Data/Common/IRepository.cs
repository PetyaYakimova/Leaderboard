namespace Leaderboard.Infrastructure.Data.Common
{
	public interface IRepository
	{
		Task AddAsync<T>(T entity) where T : class;

		IQueryable<T> All<T>() where T : class;

		IQueryable<T> AllAsReadOnly<T>() where T : class;

		Task<T?> GetByIdAsync<T>(object id) where T : class;

		Task DeleteAsync<T>(object id) where T : class;

		void Delete<T>(T entity) where T : class;

		void DeleteRange<T>(IEnumerable<T> entities) where T : class;

		Task<int> SaveChangesAsync();
	}
}
