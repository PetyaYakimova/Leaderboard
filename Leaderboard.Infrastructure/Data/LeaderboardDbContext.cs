using Leaderboard.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Infrastructure.Data
{
    public class LeaderboardDbContext : IdentityDbContext
    {
        public LeaderboardDbContext(DbContextOptions<LeaderboardDbContext> options)
            : base(options)
        {
        }

        public DbSet<Organization> Organizations { get; set; } = null!;

		public DbSet<Contest> Contests { get; set; } = null!;

		public DbSet<Team> Teams { get; set; } = null!;

		public DbSet<Point> Points { get; set; } = null!;
    }
}