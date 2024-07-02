using Leaderboard.Core.Models.Organization;

namespace Leaderboard.Core.Contracts
{
	public interface IOrganizationService
	{
		Task<OrganizationPreviewModel> GetOrganizationInfoAsync(Guid organizationId);
	}
}
