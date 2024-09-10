using Leaderboard.Infrastructure.Data;
using Leaderboard.Infrastructure.Data.Common;
using Leaderboard.Infrastructure.Data.Models;
using Leaderboard.Tests.Mocks;
using Microsoft.AspNetCore.Identity;

namespace Leaderboard.Tests.UnitTests
{
	public class UnitTestsBase
	{
		protected LeaderboardDbContext data;
		protected IRepository repository;

		[SetUp]
		public void SetUpBeforeEveryTest()
		{
			this.data = DatabaseMock.Instance;

			this.repository = new Repository(this.data);

			this.SeedDatabase();
		}

		[TearDown]
		public void TearDownBase()
		{
			data.Dispose();
		}

		public Organization MainOrganization { get; private set; } = null!;

		public ApplicationUser MainUser { get; private set; } = null!;

		public ApplicationUser SecondaryUserWhoCannotAddUsers { get; private set; } = null!;

		private void SeedDatabase()
		{
			this.SeedOrganizations();
			this.SeedUsers();

			data.SaveChanges();
		}

		private void SeedOrganizations()
		{
			MainOrganization = new Organization()
			{
				Id = Guid.NewGuid(),
				Name = "Main organization"
			};
			data.Organizations.Add(MainOrganization);
		}

		private void SeedUsers()
		{
			MainUser = new ApplicationUser()
			{
				Id = "FirstUserId",
				Email = "main.user@mail.com",
				UserName = "main.user@mail.com",
				OrganizationId = MainOrganization.Id,
				Organization = MainOrganization,
				CanAddUsers = true,
			};
			data.Users.Add(MainUser);

			SecondaryUserWhoCannotAddUsers = new ApplicationUser()
			{
				Id = "SecondaryUserId",
				Email = "secondary.user@mail.com",
				UserName = "secondary.user@mail.com",
				OrganizationId = MainOrganization.Id,
				Organization = MainOrganization,
				CanAddUsers = false,
			};
			data.Users.Add(SecondaryUserWhoCannotAddUsers);
		}
	}
}
