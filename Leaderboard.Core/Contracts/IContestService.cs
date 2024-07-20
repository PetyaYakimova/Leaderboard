using Leaderboard.Core.Models.Contest;
using static Leaderboard.Core.Constants.LimitConstants;

namespace Leaderboard.Core.Contracts
{
	public interface IContestService
	{
		Task<Guid> CreateContestAsync(ContestFormViewModel model, Guid organizationId);

		Task<bool> ContestExistsByIdAsync(Guid contestId);

		Task<bool> ContestExistsForOrganizationsByIdAsync(Guid contestId, Guid organizationId);

		Task<ContestQueryServiceModel> GetAllContestsForOrganizationAsync(Guid organizationId, string? searchedTerm = null, int? searchedNumberOfTeams = null, int currentPage = 1, int itemsPerPage = DefaultNumberOfItemsPerPage);

		Task ChangeContestStatusAsync(Guid id);

		Task<ContestFormViewModel> GetContestByIdAsync(Guid id);

		Task EditContestAsync(Guid id, ContestFormViewModel model);

		Task<bool> ContestIsActiveAsync(Guid id);

		Task<bool> ContestHasNoTeamsAsync(Guid id);

		Task<ContestTableViewModel> GetContestForPreviewAsync(Guid id);

		Task DeleteContestAsync(Guid id);

		Task<ContestDetailsViewModel> GetContestDetailsAsync(Guid id);

		Task<ContestLeaderboardViewModel> GetContestLeaderboardAsync(Guid id);

		Task CreateTeamAsync(TeamFormViewModel model, Guid contestId);

		Task<bool> TeamExistsByIdAsync(Guid teamId);

		Task<bool> TeamExistsForOrganizationsByIdAsync(Guid teamId, Guid organizationId);

		Task<TeamFormViewModel> GetTeamByIdAsync(Guid id);

		Task EditTeamAsync(Guid id, TeamFormViewModel model);

		Task<Guid> GetContestForTeamByIdAsync(Guid teamId);

		Task ChangeTeamStatusAsync(Guid id);

		Task DeleteTeamAsync(Guid id);

		Task<TeamForDeleteViewModel> GetTeamForDeleteByIdAsync(Guid id);
	}
}
