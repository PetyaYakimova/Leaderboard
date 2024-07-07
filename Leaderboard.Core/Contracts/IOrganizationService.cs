﻿using Leaderboard.Core.Models.Organization;
using static Leaderboard.Core.Constants.LimitConstants;

namespace Leaderboard.Core.Contracts
{
	public interface IOrganizationService
	{
		Task<OrganizationPreviewModel> GetOrganizationInfoAsync(Guid organizationId);

		Task<bool> OrganizationExistsByIdAsync(Guid organizationId);

		Task<Guid> GetUserOrganizationIdAsync(string userId);

        Task<Guid> CreateOrganizationAsync(string organizationName);

		Task<UserQueryServiceModel> GetAllUsersAsync(Guid organizationId, string? searchedTerm = null, int currentPage = 1, int itemsPerPage = DefaultNumberOfItemsPerPage);
    }
}
