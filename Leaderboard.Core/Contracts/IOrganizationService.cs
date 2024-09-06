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
		/// If the organization doesn't exists - EntotyNotFound exception is thrown.
		/// </summary>
		/// <param name="model">User form view model</param>
		/// <param name="organizationId">Id of the organization</param>
		/// <returns></returns>
		Task AddUserAsync(UserFormViewModel model, Guid organizationId);

		Task<OrganizationPreviewModel> GetOrganizationInfoAsync(Guid organizationId);

		Task<bool> OrganizationExistsByIdAsync(Guid organizationId);

		Task<Guid> GetUserOrganizationIdAsync(string userId);

		Task<UserQueryServiceModel> GetAllUsersAsync(Guid organizationId, string? searchedTerm = null, int currentPage = 1, int itemsPerPage = DefaultNumberOfItemsPerPage);

		Task<bool> CanUserAddUsersAsync(string userId);
	}
}
