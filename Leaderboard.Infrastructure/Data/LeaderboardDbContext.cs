using Leaderboard.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Infrastructure.Data
{
	public class LeaderboardDbContext : IdentityDbContext<ApplicationUser>
	{
		public LeaderboardDbContext(DbContextOptions<LeaderboardDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Point>(entity =>
			{
				entity.HasOne(p => p.AddedByUser)
					.WithMany(u => u.AddedPoints)
					.OnDelete(DeleteBehavior.Restrict);
			});

			base.OnModelCreating(builder);
		}


		public DbSet<Organization> Organizations { get; set; } = null!;

		public DbSet<Contest> Contests { get; set; } = null!;

		public DbSet<Team> Teams { get; set; } = null!;

		public DbSet<Point> Points { get; set; } = null!;
	}
}