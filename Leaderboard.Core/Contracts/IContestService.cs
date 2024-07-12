using Leaderboard.Core.Models.Contest;

namespace Leaderboard.Core.Contracts
{
	public interface IContestService
	{
		Task<Guid> CreateContestAsync(ContestFormViewModel model, Guid organizationId);

		Task<bool> ContestExistsByIdAsync(Guid contestId);

		Task<bool> ContestExistsForOrganizationsByIdAsync(Guid contestId, Guid organizationId);
	}
}
