﻿using Leaderboard.Core.Models.Contest;
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

		/// <summary>
		/// Creates a points record for the selected team from the selected user.
		/// If the team or the user doesn't exist - an EntityNotFound exception is thrown.
		/// If the user is not in the same organization as the team - InvalidOperationException is thrown.
		/// </summary>
		/// <param name="model">Point form view model</param>
		/// <param name="teamId">Id of the team that gets the points</param>
		/// <param name="userId">Id of the user who adds the points</param>
		/// <returns></returns>
		Task CreatePointAsync(PointFormViewModel model, Guid teamId, string userId);
		#endregion

		//Read
		#region
		/// <summary>
		/// Checks if a contest with the given id exists.
		/// </summary>
		/// <param name="contestId">Id of the contest</param>
		/// <returns></returns>
		Task<bool> ContestExistsByIdAsync(Guid contestId);

		/// <summary>
		/// Checks if a contest with given id exists for organization with the given id.
		/// </summary>
		/// <param name="contestId">Id of the contest</param>
		/// <param name="organizationId">Id of the organization</param>
		/// <returns></returns>
		Task<bool> ContestExistsForOrganizationsByIdAsync(Guid contestId, Guid organizationId);

		/// <summary>
		/// Checks if the given contest has no teams.
		/// If a contest with the given id doesn't exist - throws an entity not found exception.
		/// </summary>
		/// <param name="id">Id of the contest</param>
		/// <returns></returns>
		Task<bool> ContestHasNoTeamsAsync(Guid id);

		/// <summary>
		/// Checks if the given contest is active.
		/// If a contest with the given id doesn't exist - throws an entity not found exception.
		/// </summary>
		/// <param name="id">Id of the contest</param>
		/// <returns></returns>
		Task<bool> ContestIsActiveAsync(Guid id);

		/// <summary>
		/// Gets all the contests for an organization based on the search criteria and returns only those of them that should be on the searched page.
		/// </summary>
		/// <param name="organizationId">Id of the organization</param>
		/// <param name="searchedTerm">Search term to filter the contests by</param>
		/// <param name="searchedNumberOfTeams">Number of teams to filter the contests by</param>
		/// <param name="currentPage">Current page of results</param>
		/// <param name="itemsPerPage">Number of items per page</param>
		/// <returns></returns>
		Task<ContestQueryServiceModel> GetAllContestsForOrganizationAsync(Guid organizationId, string? searchedTerm = null, int? searchedNumberOfTeams = null, int currentPage = 1, int itemsPerPage = DefaultNumberOfItemsPerPage);

		/// <summary>
		/// Returns contest form view model for the given contest.
		/// If a contest with the given id doesn't exist - throws an entity not found exception.
		/// </summary>
		/// <param name="id">Id of the contest</param>
		/// <returns></returns>
		Task<ContestFormViewModel> GetContestByIdAsync(Guid id);

		/// <summary>
		/// Returns contest details view model for the given contest.
		/// If a contest with the given id doesn't exist - throws an entity not found exception.
		/// </summary>
		/// <param name="id">Id of the contest</param>
		/// <returns></returns>
		Task<ContestDetailsViewModel> GetContestDetailsAsync(Guid id);

		/// <summary>
		/// Returns contest for preview view model for the given contest.
		/// If a contest with the given id doesn't exist - throws an entity not found exception.
		/// </summary>
		/// <param name="id">Id of the contest</param>
		/// <returns></returns>
		Task<ContestForPreviewViewModel> GetContestForPreviewAsync(Guid id);

		/// <summary>
		/// Gets the id of the contest in which is the team.
		/// If a team with the given id doesn't exist - throws an entity not found exception.
		/// </summary>
		/// <param name="teamId">Id of the team</param>
		/// <returns></returns>
		Task<Guid> GetContestForTeamByIdAsync(Guid teamId);

		/// <summary>
		/// Returns a contest leaderboard view model for the given contest.
		/// If a contest with the given id doesn't exist - throws an entity not found exception.
		/// </summary>
		/// <param name="id">Id of the contest</param>
		/// <returns></returns>
		Task<ContestLeaderboardViewModel> GetContestLeaderboardAsync(Guid id);

		/// <summary>
		/// Returns a model with the user's pinned and unpinned contests.
		/// If a user wuth the given id doesn't exists - throws an entity not found exception.
		/// </summary>
		/// <param name="userId">Id of the user</param>
		/// <returns></returns>
		Task<PinnedContestsViewModel> GetUserPinnedAndUnpinnedContests(string userId);

		/// <summary>
		/// Checks if a team with the given id exists.
		/// </summary>
		/// <param name="teamId">Id of the team</param>
		/// <returns></returns>
		Task<bool> TeamExistsByIdAsync(Guid teamId);

		/// <summary>
		/// Checks if a team with the given id exists for the given organization.
		/// </summary>
		/// <param name="teamId">Id of the team</param>
		/// <param name="organizationId">Id of the organization</param>
		/// <returns></returns>
		Task<bool> TeamExistsForOrganizationsByIdAsync(Guid teamId, Guid organizationId);

		/// <summary>
		/// Checks if the team with the given id is active.
		/// If a team with the given id doesn't exist - throws an entity not found exception.
		/// </summary>
		/// <param name="id">Id of the team</param>
		/// <returns></returns>
		Task<bool> TeamIsActiveAsync(Guid id);

		/// <summary>
		/// Returns team form view model for the given team.
		/// If a team with the given id doesn't exist - throws an entity not found exception. 
		/// </summary>
		/// <param name="id">Id of the team</param>
		/// <returns></returns>
		Task<TeamFormViewModel> GetTeamByIdAsync(Guid id);

		/// <summary>
		/// Returns team for delete view model for the given team.
		/// If a team with the given id doesn't exist - throws an entity not found exception.
		/// </summary>
		/// <param name="id">Id of the team</param>
		/// <returns></returns>
		Task<TeamForDeleteViewModel> GetTeamForDeleteByIdAsync(Guid id);

		/// <summary>
		/// Returns the name of the given team.
		/// If a team with the given id doesn't exist - throws an entity not found exception.
		/// </summary>
		/// <param name="id">Id of the team</param>
		/// <returns></returns>
		Task<string> GetTeamNameByIdAsync(Guid id);

		/// <summary>
		/// Gets all the points for a team based on the search criteria and returns only those of them that should be on the searched page.
		/// </summary>
		/// <param name="teamId">Id of the team</param>
		/// <param name="searchedTerm"></param>
		/// <param name="searchedPoints"></param>
		/// <param name="searchedUserEmail"></param>
		/// <param name="currentPage"></param>
		/// <param name="itemsPerPage"></param>
		/// <returns></returns>
		Task<PointsQueryServiceModel> GetAllTeamPointsAsync(Guid teamId, string? searchedTerm = null, int? searchedPoints = null, string? searchedUserEmail = null, int currentPage = 1, int itemsPerPage = DefaultNumberOfItemsPerPage);
		#endregion

		//Update
		#region
		/// <summary>
		/// Changes the status of the given contest.
		/// If the contest doesn't exist - an EntityNotFound exception is thrown.
		/// </summary>
		/// <param name="id">Id of the contest</param>
		/// <returns></returns>
		Task ChangeContestStatusAsync(Guid id);

		/// <summary>
		/// Edits the details for the given contest with those from the model.
		/// If the contest doesn't exist - an EntityNotFound exception is thrown.
		/// </summary>
		/// <param name="id">Id of the contest</param>
		/// <param name="model">Contest form view model with the new contest data</param>
		/// <returns></returns>
		Task EditContestAsync(Guid id, ContestFormViewModel model);

		/// <summary>
		/// Changes the status of the given team.
		/// If the team doesn't exist - an EntityNotFound exception is thrown.
		/// </summary>
		/// <param name="id">Id of the team</param>
		/// <returns></returns>
		Task ChangeTeamStatusAsync(Guid id);

		/// <summary>
		/// Edits the details for the given team with those from the model.
		/// If the team doesn't exist - an EntityNotFound exception is thrown.
		/// </summary>
		/// <param name="id">Id of the team</param>
		/// <param name="model">Team form view model with the new details</param>
		/// <returns></returns>
		Task EditTeamAsync(Guid id, TeamFormViewModel model);
		#endregion

		//Delete
		#region
		/// <summary>
		/// Deletes the given contest and any pins for this contest.
		/// If the contest doesn't exist - an EntityNotFound exception is thrown.
		/// If the contest has teams - an CannotDeleteException is thrown.
		/// </summary>
		/// <param name="id">Id of the contest</param>
		/// <returns></returns>
		Task DeleteContestAsync(Guid id);

		/// <summary>
		/// Unpins the given contest for the given user.
		/// If the contest or the user doesn't exist - an EntityNotFound exception is thrown.
		/// If the contest is not  pinned for the user - an InvalidOperationException is thrown.
		/// </summary>
		/// <param name="contestId">Id of the pinned contest</param>
		/// <param name="userId">Id of the user</param>
		/// <returns></returns>
		Task UnpinContestForUser(Guid contestId, string userId);

		/// <summary>
		/// Deletes the given team and any points for it.
		/// If the team doesn't exist - an EntityNotFound exception is thrown.
		/// </summary>
		/// <param name="id">Id of the team</param>
		/// <returns></returns>
		Task DeleteTeamAsync(Guid id);
		#endregion
	}
}
