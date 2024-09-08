using Leaderboard.Core.Models.Organization;
using static Leaderboard.Core.Constants.LimitConstants;

namespace Leaderboard.Core.Contracts
{
	public interface IOrganizationService
	{
		/// <summary>
		/// Creates a new organization with the given name.
		/// </summary>
		/// <param name="organizationName">Organization name</param>
		/// <returns></returns>
		Task<Guid> CreateOrganizationAsync(string organizationName);

		/// <summary>
		/// Adds a new user with the details from the model in the given organization.
		/// If the organization doesn't exists - EntityNotFound exception is thrown.
		/// </summary>
		/// <param name="model">User form view model</param>
		/// <param name="organizationId">Id of the organization</param>
		/// <returns></returns>
		Task AddUserAsync(UserFormViewModel model, Guid organizationId);

		/// <summary>
		/// Returns Organization preview model for the given organization.
		/// If the organization doesn't exists - EntityNotFound exception is thrown.
		/// </summary>
		/// <param name="organizationId">Id of the organization</param>
		/// <returns></returns>
		Task<OrganizationPreviewModel> GetOrganizationInfoAsync(Guid organizationId);

		/// <summary>
		/// Checks if an organization with the given id exists.
		/// </summary>
		/// <param name="organizationId">Id of the organization</param>
		/// <returns></returns>
		Task<bool> OrganizationExistsByIdAsync(Guid organizationId);

		/// <summary>
		/// Returns organization id of the given user.
		/// If the user doesn't exists - EntityNotFound exception is thrown.
		/// </summary>
		/// <param name="userId">Id of the user</param>
		/// <returns></returns>
		Task<Guid> GetUserOrganizationIdAsync(string userId);

		/// <summary>
		/// Returns user query service model with all the users filtlered by the search criteria and that should be on the given page of results.
		/// </summary>
		/// <param name="organizationId">Id of the organization</param>
		/// <param name="searchedTerm"></param>
		/// <param name="currentPage"></param>
		/// <param name="itemsPerPage"></param>
		/// <returns></returns>
		Task<UserQueryServiceModel> GetAllUsersAsync(Guid organizationId, string? searchedTerm = null, int currentPage = 1, int itemsPerPage = DefaultNumberOfItemsPerPage);

		Task<bool> CanUserAddUsersAsync(string userId);
	}
}
