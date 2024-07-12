using Leaderboard.Core.Contracts;
using Leaderboard.Core.Services;
using Leaderboard.Infrastructure.Data;
using Leaderboard.Infrastructure.Data.Common;
using Leaderboard.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using static Leaderboard.Infrastructure.Constants.DataConstants;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped<IOrganizationService, OrganizationService>();
			services.AddScoped<IContestService, ContestService>();

			return services;
		}

		public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration config)
		{
			var connectionString = config.GetConnectionString("DefaultConnection");
			services.AddDbContext<LeaderboardDbContext>(options =>
				options.UseSqlServer(connectionString));

			services.AddScoped<IRepository, Repository>();

			services.AddDatabaseDeveloperPageExceptionFilter();

			return services;
		}

		public static IServiceCollection AddApplicationIdentity(this IServiceCollection services, IConfiguration config)
		{
			services
				.AddDefaultIdentity<ApplicationUser>(options =>
				{
					options.SignIn.RequireConfirmedAccount = false;
					options.Password.RequiredLength = UserPasswordMinLength;
					options.Password.RequireDigit = false;
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequireUppercase = false;

				})
				.AddEntityFrameworkStores<LeaderboardDbContext>();

			return services;
		}
	}
}
