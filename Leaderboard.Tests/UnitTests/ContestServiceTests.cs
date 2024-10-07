using Leaderboard.Core.Contracts;
using Leaderboard.Core.Exceptions;
using Leaderboard.Core.Models.Contest;
using Leaderboard.Core.Models.Organization;
using Leaderboard.Core.Services;
using Leaderboard.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net.WebSockets;

namespace Leaderboard.Tests.UnitTests
{
	[TestFixture]
	public class ContestServiceTests : UnitTestsBase
	{
		private IOrganizationService organizationService;
		private IContestService contestService;

		private ILogger<OrganizationService> organizationLogger;
		private ILogger<ContestService> logger;

		[SetUp]
		public void Setup()
		{
			var mockOrganizationLogger = new Mock<ILogger<OrganizationService>>();
			this.organizationLogger = mockOrganizationLogger.Object;

			var mockLogger = new Mock<ILogger<ContestService>>();
			this.logger = mockLogger.Object;

			this.organizationService = new OrganizationService(this.organizationLogger, repository);

			this.contestService = new ContestService(this.logger, repository, organizationService);
		}

		[Test]
		public async Task CeateContest_ShouldCreateContestForValidOrganizationId()
		{
			var contestsCountBefore = this.data.Contests.Count(c => c.OrganizationId == MainOrganization.Id);

			ContestFormViewModel newContest = new ContestFormViewModel()
			{
				Name = "Test",
				Description = "Some description",
				IsActive = true
			};

			await contestService.CreateContestAsync(newContest, MainOrganization.Id);

			var contestsCountAfter = this.data.Contests.Count(c => c.OrganizationId == MainOrganization.Id);
			Assert.That(contestsCountAfter, Is.EqualTo(contestsCountBefore + 1));

			var contest = this.data.Contests.FirstOrDefault(c => c.Name == newContest.Name);
			Assert.IsNotNull(contest);
			Assert.That(contest.Description, Is.EqualTo(newContest.Description));
			Assert.That(contest.IsActive, Is.EqualTo(newContest.IsActive));
		}

		[Test]
		public void CreateContest_ShouldThrowExceptionIfOrganizationDoesntExist()
		{
			Assert.That(async () => await contestService.CreateContestAsync(new ContestFormViewModel(), Guid.NewGuid()),
				Throws.Exception.TypeOf<EntityNotFoundException>());
		}

		[Test]
		public async Task PinContestForUser_ShouldPinTheContestForValidUserInTheSameOrganization()
		{
			var pinnedContestsBefore = this.data.PinnedContest.Count(c => c.ContestId == MainContest.Id);

			await contestService.PinContestForUser(MainContest.Id, MainUser.Id);

			var pinnedContestsAfter = this.data.PinnedContest.Count(c => c.ContestId == MainContest.Id);
			Assert.That(pinnedContestsAfter, Is.EqualTo(pinnedContestsBefore + 1));
		}

		[Test]
		public void PinContestForUser_ShouldThrowExceptionIfUserDoesntExist()
		{
			Assert.That(async () => await contestService.PinContestForUser(MainContest.Id, "InvalidUserId"),
				Throws.Exception.TypeOf<EntityNotFoundException>());
		}

		[Test]
		public void PinContestForUser_ShouldThrowExceptionIfContestDoesntExist()
		{
			Assert.That(async () => await contestService.PinContestForUser(Guid.NewGuid(), MainUser.Id),
				Throws.Exception.TypeOf<EntityNotFoundException>());
		}

		[Test]
		public void PinContestForUser_ShouldThrowExceptionIfContestAndUserAreInDifferentOrganizations()
		{
			Assert.That(async () => await contestService.PinContestForUser(MainContest.Id, UserInAnotherOrganization.Id),
				Throws.Exception.TypeOf<InvalidOperationException>());
		}

		[Test]
		public void PinContestForUser_ShouldThrowExceptionIfUserAlreadyHasPinnedThisContest()
		{
			PinnedContest pc = new PinnedContest()
			{
				UserId = MainUser.Id,
				ContestId = MainContest.Id
			};

			data.PinnedContest.Add(pc);


			Assert.That(async () => await contestService.PinContestForUser(MainContest.Id, MainUser.Id),
				Throws.Exception.TypeOf<InvalidOperationException>());
		}

		[Test]
		public void PinContestForUser_ShouldThrowExceptionIfContestIsInactive()
		{
			Assert.That(async () => await contestService.PinContestForUser(InactiveContest.Id, MainUser.Id),
				Throws.Exception.TypeOf<InvalidOperationException>());
		}

		[Test]
		public async Task CreateTeam_ShouldCreateTheTeamForExistingContest()
		{
			var numberOfTeamsBefore = this.data.Teams.Count(t => t.ContestId == MainContest.Id);

			TeamFormViewModel model = new TeamFormViewModel()
			{
				Name = "New team",
				Notes = "Some notes",
				IsActive = true,
				NumberOfMembers = 15,
			};

			await contestService.CreateTeamAsync(model, MainContest.Id);

			var numberOfTeamsAfter = this.data.Teams.Count(t => t.ContestId == MainContest.Id);
			Assert.That(numberOfTeamsAfter, Is.EqualTo(numberOfTeamsBefore + 1));

			var team = this.data.Teams.FirstOrDefault(t => t.Name == model.Name);
			Assert.IsNotNull(team);
			Assert.That(team.IsActive, Is.EqualTo(model.IsActive));
			Assert.That(team.Notes, Is.EqualTo(model.Notes));
			Assert.That(team.NumberOfMembers, Is.EqualTo(model.NumberOfMembers));
		}

		[Test]
		public void CreateTeam_ShouldThrowExceptionIfContestDoesntExist()
		{
			Assert.That(async () => await contestService.CreateTeamAsync(new TeamFormViewModel(), Guid.NewGuid()),
				Throws.Exception.TypeOf<EntityNotFoundException>());
		}

		[Test]
		public async Task CreatePoint_ShouldAddThePointsForAValidTeam()
		{
			var pointRecordsBefore = this.data.Points.Count(p => p.TeamId == MainTeam.Id);

			PointFormViewModel model = new PointFormViewModel()
			{
				Points = 5,
				Description = "Unit testing"
			};

			await contestService.CreatePointAsync(model, MainTeam.Id, MainUser.Id);

			var pointRecordsAfter = this.data.Points.Count(p => p.TeamId == MainTeam.Id);
			Assert.That(pointRecordsAfter, Is.EqualTo(pointRecordsBefore + 1));

			var points = this.data.Points.FirstOrDefault(p => p.TeamId == MainTeam.Id);
			Assert.IsNotNull(points);
			Assert.That(points.Points, Is.EqualTo(model.Points));
			Assert.That(points.Description, Is.EqualTo(model.Description));
		}

		[Test]
		public void CreatePoint_ShouldThrowExceptionIfTeamDoesntExist()
		{
			Assert.That(async () => await contestService.CreatePointAsync(new PointFormViewModel(), Guid.NewGuid(), MainUser.Id),
				Throws.Exception.TypeOf<EntityNotFoundException>());
		}

		[Test]
		public void CreatePoint_ShouldThrowExceptionIfUserDoesntExist()
		{
			Assert.That(async () => await contestService.CreatePointAsync(new PointFormViewModel(), MainTeam.Id, "InvalidUserId"),
				Throws.Exception.TypeOf<EntityNotFoundException>());
		}

		[Test]
		public void CreatePoint_ShouldThrowExceptionIfUserAndTeamAreInDifferentOrganizations()
		{
			Assert.That(async () => await contestService.CreatePointAsync(new PointFormViewModel(), MainTeam.Id, UserInAnotherOrganization.Id),
				Throws.Exception.TypeOf<InvalidOperationException>());
		}

		[Test]
		public async Task ContestExist_ShouldReturnTrueWhenTheContestExists()
		{
			var result = await contestService.ContestExistsByIdAsync(MainContest.Id);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task ContestExist_ShouldReturnFalseWhenTheContestDoesntExist()
		{
			var result = await contestService.ContestExistsByIdAsync(Guid.NewGuid());

			Assert.IsFalse(result);
		}

		[Test]
		public async Task ContestExistForOrganization_ShouldReturnTrueWhenTheContestExistsForTheOrganization()
		{
			var result = await contestService.ContestExistsForOrganizationsByIdAsync(MainContest.Id, MainOrganization.Id);

			Assert.IsTrue(result);
		}


		[Test]
		public async Task ContestExistForOrganization_ShouldReturnFalseWhenTheContestDoesntExistForThisOrganization()
		{
			var result = await contestService.ContestExistsForOrganizationsByIdAsync(MainContest.Id, Guid.NewGuid());

			Assert.IsFalse(result);
		}

		[Test]
		public async Task ContestHasNoTeams_ShouldReturnFalseWhenContestHasTeams()
		{
			var result = await contestService.ContestHasNoTeamsAsync(MainContest.Id);

			Assert.IsFalse(result);
		}

		[Test]
		public async Task ContestHasNoTeams_ShouldReturnTrueWhenContestHasNoTeams()
		{
			var result = await contestService.ContestHasNoTeamsAsync(InactiveContest.Id);

			Assert.IsTrue(result);
		}

		[Test]
		public void ContestHaNoTeams_ShouldThrowExceptionIfContestDoesntExist()
		{
			Assert.That(async () => await contestService.ContestHasNoTeamsAsync(Guid.NewGuid()),
				Throws.Exception.TypeOf<EntityNotFoundException>());
		}

		[Test]
		public async Task ContestIsActive_ShouldReturnTrueWhenContestIsActive()
		{
			var result = await contestService.ContestIsActiveAsync(MainContest.Id);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task ContestIsActive_ShouldReturnFalseWhenContestIsNotActive()
		{
			var result = await contestService.ContestIsActiveAsync(InactiveContest.Id);

			Assert.IsFalse(result);
		}

		[Test]
		public void ContestIsActive_ShouldThrowExceptionIfContestDoesntExist()
		{
			Assert.That(async () => await contestService.ContestIsActiveAsync(Guid.NewGuid()),
				Throws.Exception.TypeOf<EntityNotFoundException>());
		}

		[Test]
		public async Task GetAllContestForOrganization_ShouldReturnTheCorrectContests()
		{
			var result = await contestService.GetAllContestsForOrganizationAsync(MainOrganization.Id, "Main", 1);

			Assert.IsNotNull(result);
			Assert.That(result.Entities.Count(), Is.EqualTo(1));
			Assert.That(result.TotalCount, Is.EqualTo(1));
		}

		[Test]
		public async Task GetContestById_ShouldReturnCorrectContestDataForExistingContest()
		{
			var result = await contestService.GetContestByIdAsync(MainContest.Id);

			Assert.IsNotNull(result);
			Assert.That(result.Name, Is.EqualTo(MainContest.Name));
			Assert.That(result.Description, Is.EqualTo(MainContest.Description));
			Assert.That(result.IsActive, Is.EqualTo(MainContest.IsActive));
		}

		[Test]
		public void GetContestById_ShouldThrowExceptionIfContestDoesntExist()
		{
			Assert.That(async () => await contestService.GetContestByIdAsync(Guid.NewGuid()),
				Throws.Exception.TypeOf<EntityNotFoundException>());
		}

		[Test]
		public async Task GetContestDetails_ShouldReturnValidDetailsForExistingContest()
		{
			var result = await contestService.GetContestDetailsAsync(MainContest.Id);

			Assert.IsNotNull(result);
			Assert.That(result.Id, Is.EqualTo(MainContest.Id));
			Assert.That(result.Name, Is.EqualTo(MainContest.Name));
			Assert.That(result.IsActive, Is.EqualTo(MainContest.IsActive));
			Assert.That(result.Teams.Count(), Is.EqualTo(1));
		}

		[Test]
		public void GetContestDetails_ShouldThrowExceptionIfContestDoesntExist()
		{
			Assert.That(async () => await contestService.GetContestDetailsAsync(Guid.NewGuid()),
				Throws.Exception.TypeOf<EntityNotFoundException>());
		}

		[Test]
		public async Task GetContestForPreview_ShouldReturnValidDetailsForExistingContest()
		{
			var result = await contestService.GetContestForPreviewAsync(MainContest.Id);

			Assert.IsNotNull(result);
			Assert.That(result.Id, Is.EqualTo(MainContest.Id));
			Assert.That(result.Name, Is.EqualTo(MainContest.Name));
		}

		[Test]
		public void GetContestForPreview_ShouldThrowExceptionIfContestDoesntExist()
		{
			Assert.That(async () => await contestService.GetContestForPreviewAsync(Guid.NewGuid()),
				Throws.Exception.TypeOf<EntityNotFoundException>());
		}

		[Test]
		public async Task GetContestForTeamById_ShouldReturnValidContestIdWhenTeamExists()
		{
			var result = await contestService.GetContestForTeamByIdAsync(MainTeam.Id);

			Assert.IsNotNull(result);
			Assert.That(result, Is.EqualTo(MainContest.Id));
		}

		[Test]
		public void GetContestForTeambyId_ShouldThrowExceptionIfTeamDoesntExist()
		{
			Assert.That(async () => await contestService.GetContestForTeamByIdAsync(Guid.NewGuid()),
				Throws.Exception.TypeOf<EntityNotFoundException>());
		}

		[Test]
		public async Task GetContestLeaderboard_ShouldReturnCorrectLeaderboardForExistingContest()
		{
			var result = await contestService.GetContestLeaderboardAsync(MainContest.Id);

			Assert.IsNotNull(result);
			Assert.That(result.Name, Is.EqualTo(MainContest.Name));
			Assert.That(result.Description, Is.EqualTo(MainContest.Description));
			Assert.That(result.NumberOfTeams, Is.EqualTo(MainContest.Teams.Count()));
			Assert.That(result.Teams.Count(), Is.EqualTo(MainContest.Teams.Count()));
		}

		[Test]
		public void GetContestLeaderboard_ShouldThrowExceptionIfContestDoesntExist()
		{
			Assert.That(async () => await contestService.GetContestLeaderboardAsync(Guid.NewGuid()),
				Throws.Exception.TypeOf<EntityNotFoundException>());
		}

		[Test]
		public async Task GetUserPinnedAndUnipinnedContests_ShouldReturnTheCorrectActiveContests()
		{
			await contestService.PinContestForUser(MainContest.Id, MainUser.Id);

			var result = await contestService.GetUserPinnedAndUnpinnedContests(MainUser.Id);

			Assert.IsNotNull(result);
			Assert.That(result.PinnedContests.Count(), Is.EqualTo(1));
			Assert.That(result.PinnedContests.First().Id, Is.EqualTo(MainContest.Id));
			Assert.That(result.UnpinnedActiveContests.Count(), Is.EqualTo(1));
			Assert.That(result.UnpinnedActiveContests.First().Id, Is.EqualTo(AnotherContest.Id));
		}

		[Test]
		public void GetUserPinnedAndUnpinnedContests_ShouldThrowExceptionIfUsertDoesntExist()
		{
			Assert.That(async () => await contestService.GetUserPinnedAndUnpinnedContests("InvalidUserId"),
				Throws.Exception.TypeOf<EntityNotFoundException>());
		}

		[Test]
		public async Task TeamExist_ShouldReturnTrueForExistingTeam()
		{
			var result = await contestService.TeamExistsByIdAsync(MainTeam.Id);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task TeamExist_ShouldReturnFalseForNonExistingTeam()
		{
			var result = await contestService.TeamExistsByIdAsync(Guid.NewGuid());

			Assert.IsFalse(result);
		}

		[Test]
		public async Task TeamExistForOrganization_ShouldReturnTrueForExistingTeamInTheOrganization()
		{
			var result = await contestService.TeamExistsForOrganizationsByIdAsync(MainTeam.Id, MainOrganization.Id);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task TeamExistForOrganization_ShouldReturnFalseForExistingTeamInAnotherOrganization()
		{
			var result = await contestService.TeamExistsForOrganizationsByIdAsync(MainTeam.Id, AnotherOrganization.Id);

			Assert.IsFalse(result);
		}
	}
}
