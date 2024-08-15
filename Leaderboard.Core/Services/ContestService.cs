using Leaderboard.Core.Contracts;
using Leaderboard.Core.Exceptions;
using Leaderboard.Core.Models.Contest;
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

		//Create
		#region
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

		public async Task PinContestForUser(Guid contestId, string userId)
		{
			var user = await repository.GetByIdAsync<ApplicationUser>(userId);

			if (user == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(ApplicationUser), userId);
				throw new EntityNotFoundException();
			}

			var contest = await repository.AllAsReadOnly<Contest>()
				.Include(c => c.PinnedByUsers)
				.Where(c => c.Id == contestId)
				.FirstOrDefaultAsync();

			if (contest == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Contest), contestId);
				throw new EntityNotFoundException();
			}

			if (contest.PinnedByUsers.Any(p => p.UserId == userId))
			{
				logger.LogError(ContestIsAlreadyPinnedForThisUserLoggerErrorMessage);
				throw new InvalidOperationException();
			}

			if (contest.IsActive == false)
			{
				logger.LogError(ContestCannotBePinnedBecauseItIsInactiveLoggerErrorMessage);
				throw new InvalidOperationException();
			}

			PinnedContest pinnedContest = new PinnedContest()
			{
				UserId = userId,
				ContestId = contestId
			};

			await repository.AddAsync(pinnedContest);
			await repository.SaveChangesAsync();
		}

		public async Task CreateTeamAsync(TeamFormViewModel model, Guid contestId)
		{
			if (await this.ContestExistsByIdAsync(contestId) == false)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Contest), contestId);
				throw new EntityNotFoundException();
			}

			Team team = new Team()
			{
				Id = new Guid(),
				Name = model.Name,
				Notes = model.Notes,
				IsActive = model.IsActive,
				NumberOfMembers = model.NumberOfMembers,
				ContestId = contestId
			};

			await repository.AddAsync(team);
			await repository.SaveChangesAsync();
		}

		public async Task CreatePointAsync(PointFormViewModel model, Guid teamId, string userId)
		{
			if (await this.TeamExistsByIdAsync(teamId) == false)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Team), teamId);
				throw new EntityNotFoundException();
			}

			if (await repository.AllAsReadOnly<ApplicationUser>().AnyAsync(u => u.Id == userId) == false)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(ApplicationUser), userId);
				throw new EntityNotFoundException();
			}

			Point point = new Point()
			{
				AddedByUserId = userId,
				Description = model.Description,
				Points = model.Points,
				TeamId = teamId
			};

			await repository.AddAsync(point);
			await repository.SaveChangesAsync();
		}
		#endregion

		//Read
		#region
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

		public async Task<bool> ContestHasNoTeamsAsync(Guid id)
		{
			var contest = await repository.AllAsReadOnly<Contest>()
				.Include(c => c.Teams)
				.Where(c => c.Id == id)
				.FirstOrDefaultAsync();

			if (contest == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Contest), id);
				throw new EntityNotFoundException();
			}

			return contest.Teams.Count() == 0;
		}

		public async Task<bool> ContestIsActiveAsync(Guid id)
		{
			var contest = await repository.GetByIdAsync<Contest>(id);

			if (contest == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Contest), id);
				throw new EntityNotFoundException();
			}

			return contest.IsActive;
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

		public async Task<ContestDetailsViewModel> GetContestDetailsAsync(Guid id)
		{
			Contest? contest = await repository.AllAsReadOnly<Contest>()
				.Include(c => c.Teams)
					.ThenInclude(t => t.Points)
				.Where(c => c.Id == id)
				.FirstOrDefaultAsync();

			if (contest == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Contest), id);
				throw new EntityNotFoundException();
			}

			return new ContestDetailsViewModel()
			{
				Id = contest.Id,
				Name = contest.Name,
				NumberOfTeams = contest.Teams.Count(),
				IsActive = contest.IsActive,
				Description = contest.Description,
				Teams = contest.Teams.Select(t => new TeamResultTableViewModel()
				{
					Id = t.Id,
					Name = t.Name,
					IsActive = t.IsActive,
					TotalPoints = t.Points.Sum(p => p.Points)
				})
				.OrderBy(t => t.Name)
			};
		}

		public async Task<ContestForPreviewViewModel> GetContestForPreviewAsync(Guid id)
		{
			Contest? contest = await repository.AllAsReadOnly<Contest>()
				.Include(c => c.Teams)
				.Where(c => c.Id == id)
				.FirstOrDefaultAsync();

			if (contest == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Contest), id);
				throw new EntityNotFoundException();
			}

			return new ContestForPreviewViewModel()
			{
				Id = contest.Id,
				Name = contest.Name
			};
		}

		public async Task<Guid> GetContestForTeamByIdAsync(Guid teamId)
		{
			var team = await repository.GetByIdAsync<Team>(teamId);
			if (team == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Team), teamId);
				throw new EntityNotFoundException();
			}

			return team.ContestId;
		}

		public async Task<ContestLeaderboardViewModel> GetContestLeaderboardAsync(Guid id)
		{
			Contest? contest = await repository.AllAsReadOnly<Contest>()
				.Include(c => c.Teams)
					.ThenInclude(t => t.Points)
				.Where(c => c.Id == id)
				.FirstOrDefaultAsync();

			if (contest == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Contest), id);
				throw new EntityNotFoundException();
			}

			return new ContestLeaderboardViewModel()
			{
				Name = contest.Name,
				NumberOfTeams = contest.Teams.Count(),
				Description = contest.Description,
				Teams = contest.Teams.Where(t => t.IsActive).Select(t => new TeamLeaderboardTableViewModel()
				{
					Name = t.Name,
					TotalPoints = t.Points.Sum(p => p.Points)
				})
				.OrderByDescending(t => t.TotalPoints)
			};
		}

		public async Task<PinnedContestsViewModel> GetUserPinnedAndUnpinnedContests(string userId)
		{
			var user = await repository.GetByIdAsync<ApplicationUser>(userId);

			if (user == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(ApplicationUser), userId);
				throw new EntityNotFoundException();
			}

			var pinnedContests = await repository.AllAsReadOnly<Contest>()
				.Where(c => c.PinnedByUsers.Any(u => u.UserId == userId))
				.Select(c => new ContestTableViewModel()
				{
					Id = c.Id,
					Name = c.Name,
					IsActive = c.IsActive,
					NumberOfTeams = c.Teams.Count()
				}).ToListAsync();

			Guid organizationId = await organizationService.GetUserOrganizationIdAsync(userId);
			var unpinnedContests = await repository.AllAsReadOnly<Contest>()
				.Where(c => c.OrganizationId == organizationId)
				.Where(c => !c.PinnedByUsers.Any(u => u.UserId == userId))
				.Where(c => c.IsActive)
				.Select(c => new ContestForPreviewViewModel()
				{
					Id = c.Id,
					Name = c.Name
				}).ToListAsync();

			return new PinnedContestsViewModel()
			{
				PinnedContests = pinnedContests,
				UnpinnedActiveContests = unpinnedContests
			};
		}

		public async Task<bool> TeamExistsByIdAsync(Guid teamId)
		{
			return await repository.AllAsReadOnly<Team>()
				.AnyAsync(t => t.Id == teamId);
		}

		public async Task<bool> TeamExistsForOrganizationsByIdAsync(Guid teamId, Guid organizationId)
		{
			return await repository.AllAsReadOnly<Team>()
				.Include(t => t.Contest)
				.Where(t => t.Id == teamId)
				.AnyAsync(t => t.Contest.OrganizationId == organizationId);
		}

		public async Task<bool> TeamIsActiveAsync(Guid id)
		{
			var team = await repository.GetByIdAsync<Team>(id);

			if (team == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Team), id);
				throw new EntityNotFoundException();
			}

			return team.IsActive;
		}

		public async Task<TeamFormViewModel> GetTeamByIdAsync(Guid id)
		{
			Team? team = await repository.GetByIdAsync<Team>(id);
			if (team == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Team), id);
				throw new EntityNotFoundException();
			}

			return new TeamFormViewModel()
			{
				Name = team.Name,
				Notes = team.Notes,
				IsActive = team.IsActive,
				NumberOfMembers = team.NumberOfMembers
			};
		}

		public async Task<TeamForDeleteViewModel> GetTeamForDeleteByIdAsync(Guid id)
		{
			var team = await repository.GetByIdAsync<Team>(id);
			if (team == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Team), id);
				throw new EntityNotFoundException();
			}

			return new TeamForDeleteViewModel()
			{
				Id = team.Id,
				Name = team.Name,
				ContestId = team.ContestId
			};
		}

		public async Task<string> GetTeamNameByIdAsync(Guid id)
		{
			Team? team = await repository.GetByIdAsync<Team>(id);
			if (team == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Team), id);
				throw new EntityNotFoundException();
			}

			return team.Name;
		}

		public async Task<PointsQueryServiceModel> GetAllTeamPointsAsync(Guid teamId, string? searchedTerm = null, int? searchedPoints = null, string? searchedUserEmail = null, int currentPage = 1, int itemsPerPage = DefaultNumberOfItemsPerPage)
		{
			var pointsToShow = repository.AllAsReadOnly<Point>()
				.Where(p => p.TeamId == teamId);

			if (searchedTerm != null)
			{
				string normalizedSearchTerm = searchedTerm.ToLower();
				pointsToShow = pointsToShow.Where(p => p.Description != null && p.Description.ToLower().Contains(normalizedSearchTerm));
			}

			if (searchedPoints != null)
			{
				pointsToShow = pointsToShow.Where(p => p.Points == searchedPoints);
			}

			if (searchedUserEmail != null)
			{
				string normalizedEmail = searchedUserEmail.ToLower();
				pointsToShow = pointsToShow.Where(p => p.AddedByUser.Email.ToLower().Contains(normalizedEmail));
			}

			var points = await pointsToShow
				.OrderByDescending(p => p.Id)
				.Skip((currentPage - 1) * itemsPerPage)
				.Take(itemsPerPage)
				.Select(p => new PointTableViewModel()
				{
					Description = p.Description,
					Points = p.Points,
					AddedByUserWithEmail = p.AddedByUser.Email
				}).ToListAsync();

			int totalPointsToShow = await pointsToShow.CountAsync();

			return new PointsQueryServiceModel()
			{
				TotalCount = totalPointsToShow,
				Entities = points
			};
		}
		#endregion

		//Update
		#region
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

		public async Task ChangeTeamStatusAsync(Guid id)
		{
			Team? team = await repository.GetByIdAsync<Team>(id);

			if (team == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Team), id);
				throw new EntityNotFoundException();
			}

			team.IsActive = !team.IsActive;

			await repository.SaveChangesAsync();
		}

		public async Task EditTeamAsync(Guid id, TeamFormViewModel model)
		{
			var team = await repository.GetByIdAsync<Team>(id);

			if (team == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Team), id);
				throw new EntityNotFoundException();
			}

			team.Name = model.Name;
			team.Notes = model.Notes;
			team.IsActive = model.IsActive;
			team.NumberOfMembers = model.NumberOfMembers;

			await repository.SaveChangesAsync();
		}
		#endregion

		//Delete
		#region
		public async Task DeleteContestAsync(Guid id)
		{
			var contest = await repository.AllAsReadOnly<Contest>()
			   .Include(c => c.Teams)
			   .Include(c => c.PinnedByUsers)
			   .Where(c => c.Id == id)
			   .FirstOrDefaultAsync();

			if (contest == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Contest), id);
				throw new EntityNotFoundException();
			}

			if (contest.Teams.Count() > 0)
			{
				logger.LogError(CannotDeleteContestWithTeamsLoggerErrorMessage);
				throw new CannotDeleteEntityException();
			}

			List<PinnedContest> pins = await repository.All<PinnedContest>()
				.Where(p => p.ContestId == id)
				.ToListAsync();

			//TODO: If this works - use it when deleting points for a team as well
			if (pins.Any())
			{
				repository.DeleteRange(pins);
			}

			await repository.DeleteAsync<Contest>(id);
			await repository.SaveChangesAsync();
		}

		public async Task UnpinContestForUser(Guid contestId, string userId)
		{
			var user = await repository.GetByIdAsync<ApplicationUser>(userId);

			if (user == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(ApplicationUser), userId);
				throw new EntityNotFoundException();
			}

			var contest = await repository.GetByIdAsync<Contest>(contestId);

			if (contest == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Contest), contestId);
				throw new EntityNotFoundException();
			}

			var pinnedContest = await repository.AllAsReadOnly<PinnedContest>()
				.Where(p => p.UserId == userId)
				.FirstOrDefaultAsync(p => p.ContestId == contestId);

			if (pinnedContest == null)
			{
				logger.LogError(ContestIsIsNotPinnedForThisUserLoggerErrorMessage);
				throw new InvalidOperationException();
			}

			repository.Delete<PinnedContest>(pinnedContest);
			await repository.SaveChangesAsync();
		}

		public async Task DeleteTeamAsync(Guid id)
		{
			var team = await repository.AllAsReadOnly<Team>()
			   .Where(t => t.Id == id)
			   .FirstOrDefaultAsync();

			if (team == null)
			{
				logger.LogError(EntityWithIdWasNotFoundLoggerErrorMessage, nameof(Team), id);
				throw new EntityNotFoundException();
			}

			List<Point> points = await repository.All<Point>()
				.Where(p => p.TeamId == id)
				.ToListAsync();

			foreach (Point point in points)
			{
				await repository.DeleteAsync<Point>(point.Id);
			}

			await repository.DeleteAsync<Team>(id);
			await repository.SaveChangesAsync();
		}
		#endregion
	}
}
