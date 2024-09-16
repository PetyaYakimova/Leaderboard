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
	}
}
