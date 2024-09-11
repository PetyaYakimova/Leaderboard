using Leaderboard.Core.Contracts;
using Leaderboard.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Leaderboard.Tests.UnitTests
{
	[TestFixture]
	public class OrganizationServiceTests : UnitTestsBase
	{
		private IOrganizationService organizationService;
		private ILogger<OrganizationService> logger;

		[SetUp]
		public void Setup()
		{
			var mockLogger = new Mock<ILogger<OrganizationService>>();
			this.logger = mockLogger.Object;

			this.organizationService = new OrganizationService(this.logger, repository);
		}

		[Test]
		public async Task CreateOrganization_ShouldCreateNewOrganization()
		{
			var organizationCountBefore = this.data.Organizations.Count();

			string organizatioName = "New organization from test";

			await organizationService.CreateOrganizationAsync(organizatioName);

			var organizationCountAfter = this.data.Organizations.Count();
			Assert.That(organizationCountAfter, Is.EqualTo(organizationCountBefore + 1));
		}
	}
}
