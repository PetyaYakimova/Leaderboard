using Leaderboard.Core.Contracts;
using Leaderboard.Core.Exceptions;
using Leaderboard.Core.Models.Organization;
using Leaderboard.Infrastructure.Data.Common;
using Leaderboard.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Leaderboard.Core.Constants.MessagesConstants;

namespace Leaderboard.Core.Services
{
	public class OrganizationService : IOrganizationService
	{
		private readonly ILogger<OrganizationService> logger;
		private readonly IRepository repository;

		public OrganizationService
			(ILogger<OrganizationService> logger,
			IRepository repository)
		{
			this.logger = logger;
			this.repository = repository;
		}

		public async Task<Guid> CreateOrganizationAsync(string organizationName)
		{
			Organization organization = new Organization()
			{
				Id = Guid.NewGuid(),
				Name = organizationName
			};

			await repository.AddAsync(organization);
			await repository.SaveChangesAsync();

			return organization.Id;
		}

		public async Task<UserQueryServiceModel> GetAllUsersAsync(Guid organizationId, string? searchTerm = null, int currentPage = 1, int itemsPerPage = 10)
		{
			var usersToShow = repository.AllAsReadOnly<ApplicationUser>()
				.Where(u => u.OrganizationId == organizationId);

			if (searchTerm != null)
			{
				string normalizedSearchTerm = searchTerm.ToLower();
				usersToShow = usersToShow.Where(u => u.Email.ToLower().Contains(normalizedSearchTerm));
			}

			var users = await usersToShow
				.OrderBy(u => u.Id)
				.Skip((currentPage - 1) * itemsPerPage)
				.Take(itemsPerPage)
				.Select(u => new UserTableViewModel()
				{
					Email = u.Email,
					CanAddUsers = u.CanAddUsers
				}).ToListAsync();

			int totalUsersToShow = await usersToShow.CountAsync();

			return new UserQueryServiceModel()
			{
				TotalCount = totalUsersToShow,
				Entities = users
			};
		}

		public async Task<OrganizationPreviewModel> GetOrganizationInfoAsync(Guid organizationId)
		{
			var model = await repository.AllAsReadOnly<Organization>()
				.Where(o => o.Id == organizationId)
				.Include(o => o.Users)
				.Include(o => o.Contests)
				.Select(o => new OrganizationPreviewModel()
				{
					Id = o.Id,
					Name = o.Name,
					NumberOfAdministrators = o.Users.Count(),
					NumberOfContests = o.Contests.Count()
				}).FirstOrDefaultAsync();

			if (model == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Organization), organizationId);
				throw new EntityNotFoundException();
			}

			return model;
		}

		public async Task<Guid> GetUserOrganizationIdAsync(string userId)
		{
			var user = await repository.AllAsReadOnly<ApplicationUser>()
				.FirstOrDefaultAsync(u => u.Id == userId);

			if (user == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(ApplicationUser), userId);
				throw new EntityNotFoundException();
			}

			return user.OrganizationId;
		}

		public async Task<bool> OrganizationExistsByIdAsync(Guid organizationId)
		{
			return await repository.AllAsReadOnly<Organization>()
				.AnyAsync(o => o.Id == organizationId);
		}
	}
}
