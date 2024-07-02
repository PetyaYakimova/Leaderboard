namespace Leaderboard.Infrastructure.Data.Common
{
	public interface IRepository
	{
		IQueryable<T> All<T>() where T : class;

		IQueryable<T> AllAsReadOnly<T>() where T : class;
	}
}
