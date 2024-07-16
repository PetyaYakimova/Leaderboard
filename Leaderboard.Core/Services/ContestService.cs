using Leaderboard.Core.Contracts;
using Leaderboard.Core.Exceptions;
using Leaderboard.Core.Models.Contest;
using Leaderboard.Core.Models.Organization;
using Leaderboard.Infrastructure.Data.Common;
using Leaderboard.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Leaderboard.Core.Constants.MessagesConstants;
using static Leaderboard.Core.Constants.LimitConstants;

namespace Leaderboard.Core.Services
{
    public class ContestService : IContestService
    {
        private readonly ILogger<ContestService> logger;
        private readonly IRepository repository;
        private readonly IOrganizationService organizationService;

        public ContestService
            (ILogger<ContestService> logger,
            IRepository repository,
            IOrganizationService organizationService)
        {
            this.logger = logger;
            this.repository = repository;
            this.organizationService = organizationService;
        }

		public async Task ChangeContestStatusAsync(Guid id)
		{
			Contest? contest = await repository.GetByIdAsync<Contest>(id);

			if (contest == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Contest), id);
				throw new EntityNotFoundException();
			}

			contest.IsActive = !contest.IsActive;

			await repository.SaveChangesAsync();
		}

		public async Task<bool> ContestExistsByIdAsync(Guid contestId)
        {
            return await repository.AllAsReadOnly<Contest>()
                .AnyAsync(c => c.Id == contestId);
        }

        public async Task<bool> ContestExistsForOrganizationsByIdAsync(Guid contestId, Guid organizationId)
        {
            return await repository.AllAsReadOnly<Contest>()
                .Where(c => c.Id == contestId)
                .AnyAsync(c => c.OrganizationId == organizationId);
        }

        public async Task<Guid> CreateContestAsync(ContestFormViewModel model, Guid organizationId)
        {
            if (await organizationService.OrganizationExistsByIdAsync(organizationId) == false)
            {
                logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Organization), organizationId);
                throw new EntityNotFoundException();
            }

            Contest contest = new Contest()
            {
                Id = new Guid(),
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive,
                OrganizationId = organizationId
            };

            await repository.AddAsync(contest);
            await repository.SaveChangesAsync();

            return contest.Id;
        }

		public async Task EditContestAsync(Guid id, ContestFormViewModel model)
		{
			var contest = await repository.GetByIdAsync<Contest>(id);

			if (contest == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Contest), id);
				throw new EntityNotFoundException();
			}

			contest.Name = model.Name;
			contest.Description = model.Description;
			contest.IsActive = model.IsActive;

			await repository.SaveChangesAsync();
		}

		public async Task<ContestQueryServiceModel> GetAllContestsForOrganizationAsync(Guid organizationId, string? searchTerm = null, int? searchNumberOfTeams = null, int currentPage = 1, int itemsPerPage = DefaultNumberOfItemsPerPage)
        {
            var contestsToShow = repository.AllAsReadOnly<Contest>()
                .Where(u => u.OrganizationId == organizationId);

            if (searchTerm != null)
            {
                string normalizedSearchTerm = searchTerm.ToLower();
                contestsToShow = contestsToShow.Where(c => c.Name.ToLower().Contains(normalizedSearchTerm));
            }

            if (searchNumberOfTeams != null)
            {
                contestsToShow = contestsToShow.Where(c => c.Teams.Count() == searchNumberOfTeams);
            }


            var contests = await contestsToShow
                .OrderBy(c => c.Name)
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Select(c => new ContestTableViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsActive = c.IsActive,
                    NumberOfTeams = c.Teams.Count()
                }).ToListAsync();

            int totalContestsToShow = await contestsToShow.CountAsync();

            return new ContestQueryServiceModel()
            {
                TotalCount = totalContestsToShow,
                Entities = contests
            };
        }

		public async Task<ContestFormViewModel> GetContestByIdAsync(Guid id)
		{
			Contest? contest = await repository.GetByIdAsync<Contest>(id);
			if (contest == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Contest), id);
				throw new EntityNotFoundException();
			}

            return new ContestFormViewModel()
            {
                Name = contest.Name,
                Description = contest.Description,
                IsActive = contest.IsActive
            };
		}
	}
}
