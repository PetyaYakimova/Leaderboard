using Leaderboard.Core.Models.Contest;
using static Leaderboard.Core.Constants.LimitConstants;

namespace Leaderboard.Core.Contracts
{
	public interface IContestService
	{
		//Create
		#region
		Task<Guid> CreateContestAsync(ContestFormViewModel model, Guid organizationId);

		Task PinContestForUser(Guid contestId, string userId);

		Task CreateTeamAsync(TeamFormViewModel model, Guid contestId);

		Task CreatePointAsync(PointFormViewModel model, Guid teamId, string userId);
		#endregion

		//Read
		#region
		Task<bool> ContestExistsByIdAsync(Guid contestId);

		Task<bool> ContestExistsForOrganizationsByIdAsync(Guid contestId, Guid organizationId);

		Task<bool> ContestHasNoTeamsAsync(Guid id);

		Task<bool> ContestIsActiveAsync(Guid id);

		Task<ContestQueryServiceModel> GetAllContestsForOrganizationAsync(Guid organizationId, string? searchedTerm = null, int? searchedNumberOfTeams = null, int currentPage = 1, int itemsPerPage = DefaultNumberOfItemsPerPage);

		Task<ContestFormViewModel> GetContestByIdAsync(Guid id);

		Task<ContestDetailsViewModel> GetContestDetailsAsync(Guid id);

		Task<ContestForPreviewViewModel> GetContestForPreviewAsync(Guid id);

		Task<Guid> GetContestForTeamByIdAsync(Guid teamId);

		Task<ContestLeaderboardViewModel> GetContestLeaderboardAsync(Guid id);

		Task<PinnedContestsViewModel> GetUserPinnedAndUnpinnedContests(string userId);

		Task<bool> TeamExistsByIdAsync(Guid teamId);

		Task<bool> TeamExistsForOrganizationsByIdAsync(Guid teamId, Guid organizationId);

		Task<bool> TeamIsActiveAsync(Guid id);

		Task<TeamFormViewModel> GetTeamByIdAsync(Guid id);

		Task<TeamForDeleteViewModel> GetTeamForDeleteByIdAsync(Guid id);

		Task<string> GetTeamNameByIdAsync(Guid id);

		Task<PointsQueryServiceModel> GetAllTeamPointsAsync(Guid teamId, string? searchedTerm = null, int? searchedPoints = null, string? searchedUserEmail = null, int currentPage = 1, int itemsPerPage = DefaultNumberOfItemsPerPage);
		#endregion

		//Update
		#region
		Task ChangeContestStatusAsync(Guid id);

		Task EditContestAsync(Guid id, ContestFormViewModel model);

		Task ChangeTeamStatusAsync(Guid id);

		Task EditTeamAsync(Guid id, TeamFormViewModel model);
		#endregion

		//Delete
		#region
		Task DeleteContestAsync(Guid id);

		Task UnpinContestForUser(Guid contestId, string userId);

		Task DeleteTeamAsync(Guid id);
		#endregion
	}
}
