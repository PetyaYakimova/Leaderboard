using Leaderboard.Core.Contracts;
using Leaderboard.Core.Exceptions;
using Leaderboard.Core.Models.Contest;
using Leaderboard.Core.Models.Organization;
using Leaderboard.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;

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
	}
}
