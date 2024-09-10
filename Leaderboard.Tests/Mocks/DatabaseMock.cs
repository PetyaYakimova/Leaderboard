using Leaderboard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Tests.Mocks
{
	public static class DatabaseMock
	{
		public static LeaderboardDbContext Instance
		{
			get
			{
				var dbContextOptions = new DbContextOptionsBuilder<LeaderboardDbContext>()
					.UseInMemoryDatabase("LeaderboardInMemoryDb" + DateTime.Now.Ticks.ToString())
					.Options;

				return new LeaderboardDbContext(dbContextOptions);
			}
		}
	}
}
