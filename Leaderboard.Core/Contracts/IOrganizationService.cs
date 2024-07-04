﻿using Leaderboard.Core.Models.Organization;

namespace Leaderboard.Core.Contracts
{
	public interface IOrganizationService
	{
		Task<OrganizationPreviewModel> GetOrganizationInfoAsync(Guid organizationId);

		Task<bool> OrganizationExistsByIdAsync(Guid organizationId);

		Task<Guid> GetUserOrganizationIdAsync(string userId);
	}
}
