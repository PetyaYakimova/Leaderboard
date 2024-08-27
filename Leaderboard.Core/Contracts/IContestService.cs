using Leaderboard.Core.Models.Contest;
using static Leaderboard.Core.Constants.LimitConstants;

namespace Leaderboard.Core.Contracts
{
	public interface IContestService
	{
		//Create
		#region
		/// <summary>
		/// Creates a contest in the given organization. 
		/// If the organization doesn't exist - an EntityNotFound exception is thrown.
		/// </summary>
		/// <param name="model">Contest form view model</param>
		/// <param name="organizationId">The organization id for the organization the contest is in</param>
		/// <returns>The id of the new contest</returns>
		Task<Guid> CreateContestAsync(ContestFormViewModel model, Guid organizationId);

		/// <summary>
		/// Adds the contest in the pinned contests for the user.
		/// If the user or the contest doesn't exist - an EntityNotFound exception is thrown. If the contest and the user are in different organizations, or if the contest is already pinned for the user, or if the contest is inactive - an InvalidOperationException is thrown.
		/// </summary>
		/// <param name="contestId">The id of the contest that is to be pinend</param>
		/// <param name="userId">The id of the user who pins the contest</param>
		/// <returns></returns>
		Task PinContestForUser(Guid contestId, string userId);

		/// <summary>
		/// Creates a new team for the selected contest.
		/// If the contest doesn't exist - an EntityNotFound exception is thrown.
		/// </summary>
		/// <param name="model">Team form view model</param>
		/// <param name="contestId">The id of the contest for the team</param>
		/// <returns></returns>
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
