using Leaderboard.Core.Contracts;
using Leaderboard.Core.Services;
using Leaderboard.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddTransient<IOrganizationService, OrganizationService>();

			return services;
		}

		public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration config)
		{
			var connectionString = config.GetConnectionString("DefaultConnection");
			services.AddDbContext<LeaderboardDbContext>(options =>
				options.UseSqlServer(connectionString));

			services.AddDatabaseDeveloperPageExceptionFilter();

			return services;
		}

		public static IServiceCollection AddApplicationIdentity(this IServiceCollection services, IConfiguration config)
		{
			services
				.AddDefaultIdentity<IdentityUser>(options =>
				{
					options.SignIn.RequireConfirmedAccount = false;
					options.Password.RequiredLength = 8;
					options.Password.RequireDigit = false;
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequireUppercase = false;

				})
				.AddEntityFrameworkStores<LeaderboardDbContext>();

			return services;
		}
	}
}
