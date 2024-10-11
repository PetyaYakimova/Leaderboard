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

		public Organization AnotherOrganization { get; private set; } = null!;

		public ApplicationUser MainUser { get; private set; } = null!;

		public ApplicationUser SecondaryUserWhoCannotAddUsers { get; private set; } = null!;

		public ApplicationUser UserInAnotherOrganization { get; private set; } = null!;

		public Contest MainContest { get; private set; } = null!;

		public Contest AnotherContest { get; private set; } = null!;

		public Contest InactiveContest { get; private set; } = null!;

		public Team MainTeam { get; private set; } = null!;

		public Team InactiveTeam { get; private set; } = null!;

		public Point MainTeamPoint { get; private set; } = null!;

		private void SeedDatabase()
		{
			this.SeedOrganizations();
			this.SeedUsers();
			this.SeedContests();
			this.SeedTeams();
			this.SeedPoints();

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

			AnotherOrganization = new Organization()
			{
				Id = Guid.NewGuid(),
				Name = "Another Organization"
			};
			data.Organizations.Add(AnotherOrganization);
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

			UserInAnotherOrganization = new ApplicationUser()
			{
				Id = "UserInAnotherOrganizationId",
				Email = "user.anotherorg@mail.com",
				UserName = "user.anotherorg@mail.com",
				OrganizationId = AnotherOrganization.Id,
				Organization = AnotherOrganization,
				CanAddUsers = true,
			};
			data.Users.Add(UserInAnotherOrganization);
		}

		private void SeedContests()
		{
			MainContest = new Contest()
			{
				Id = Guid.NewGuid(),
				Name = "Main contest",
				OrganizationId = MainOrganization.Id,
				Organization = MainOrganization,
				Description = "Some contest",
				IsActive = true
			};
			data.Contests.Add(MainContest);

			AnotherContest = new Contest()
			{
				Id = Guid.NewGuid(),
				Name = "Another contest",
				OrganizationId = MainOrganization.Id,
				Organization = MainOrganization,
				Description = "Some other contest",
				IsActive = true
			};
			data.Contests.Add(AnotherContest);

			InactiveContest = new Contest()
			{
				Id = Guid.NewGuid(),
				Name = "Inactive contest",
				OrganizationId = MainOrganization.Id,
				Organization = MainOrganization,
				Description = "Some contest",
				IsActive = false
			};
			data.Contests.Add(InactiveContest);
		}

		private void SeedTeams()
		{
			MainTeam = new Team()
			{
				Id = Guid.NewGuid(),
				Name = "Main team",
				Notes = "Some notes for the team",
				IsActive = true,
				NumberOfMembers = 10,
				ContestId = MainContest.Id,
				Contest = MainContest
			};
			data.Teams.Add(MainTeam);

			InactiveTeam = new Team()
			{
				Id = Guid.NewGuid(),
				Name = "Inactive team",
				Notes = "Some notes for the inactive team",
				IsActive = false,
				NumberOfMembers = 1,
				ContestId = MainContest.Id,
				Contest = MainContest
			};
			data.Teams.Add(InactiveTeam);
		}

		public void SeedPoints()
		{
			MainTeamPoint = new Point()
			{
				Id = 1,
				TeamId = MainTeam.Id,
				Team = MainTeam,
				Points = 15,
				Description = "Some description",
				AddedByUserId = MainUser.Id,
				AddedByUser = MainUser
			};
			data.Points.Add(MainTeamPoint);
		}
	}
}
