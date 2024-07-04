using Leaderboard.Core.Contracts;
using Leaderboard.Core.Models.Organization;
using Leaderboard.Infrastructure.Data.Common;
using Leaderboard.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Core.Services
{
	public class OrganizationService : IOrganizationService
	{
		private readonly IRepository repository;

		public OrganizationService(IRepository repository)
		{
			this.repository = repository;
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
				//TODO: Change with custom exception
				throw new ArgumentNullException();
			}

			return model;
		}

		public async Task<Guid?> GetUserOrganizationIdAsync(string userId)
		{
			var user = await repository.AllAsReadOnly<IdentityUser>()
				.FirstOrDefaultAsync(u => u.Id == userId);

			if (user == null)
			{
				//TODO: Change with custom exception
				throw new ArgumentNullException();
			}

			//TODO: Fix this when we have the organization Id for the extended user.
			return new Guid();
			//return user.OrganizationId;
		}

		public async Task<bool> OrganizationExistsByIdAsync(Guid organizationId)
		{
			return await repository.AllAsReadOnly<Organization>()
				.AnyAsync(o => o.Id == organizationId);
		}
	}
}
