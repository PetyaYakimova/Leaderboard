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

	}
}
