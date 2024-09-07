﻿using Leaderboard.Core.Models.Organization;
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
		/// Check if an organization with the given id exists.
		/// </summary>
		/// <param name="organizationId">Id of the organization</param>
		/// <returns></returns>
		Task<bool> OrganizationExistsByIdAsync(Guid organizationId);

		Task<Guid> GetUserOrganizationIdAsync(string userId);

		Task<UserQueryServiceModel> GetAllUsersAsync(Guid organizationId, string? searchedTerm = null, int currentPage = 1, int itemsPerPage = DefaultNumberOfItemsPerPage);

		Task<bool> CanUserAddUsersAsync(string userId);
	}
}
