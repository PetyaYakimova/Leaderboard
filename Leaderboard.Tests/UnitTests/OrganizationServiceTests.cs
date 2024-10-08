﻿using Leaderboard.Core.Contracts;
using Leaderboard.Core.Exceptions;
using Leaderboard.Core.Models.Organization;
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

		[Test]
		public async Task AddUser_ShouldAddTheUserToExistingOrganization() 
		{
			var userCountBefore = this.data.Users.Count(u => u.OrganizationId == MainOrganization.Id);

			UserFormViewModel userModel = new UserFormViewModel()
			{
				CanAddUsers = true,
				Email = "new_user@mail.bg",
				Password = "password"
			};

			await organizationService.AddUserAsync(userModel, MainOrganization.Id);

			var userCountAfter = this.data.Users.Count(u => u.OrganizationId == MainOrganization.Id);
			Assert.That(userCountAfter, Is.EqualTo(userCountBefore + 1));

			var allUsers = await organizationService.GetAllUsersAsync(MainOrganization.Id);
			var user = allUsers.Entities.FirstOrDefault(u => u.Email == userModel.Email);
			Assert.IsNotNull(user);
			Assert.That(user.CanAddUsers, Is.EqualTo(userModel.CanAddUsers));
		}

		[Test]
		public void AddUser_ShouldThrowExceptionIfOrganizationDoesntExist()
		{
			Assert.That(async () => await organizationService.AddUserAsync(new UserFormViewModel(), Guid.NewGuid()),
				Throws.Exception.TypeOf<EntityNotFoundException>());
		}

		[Test]
		public async Task GetOrganizationInfo_ShouldReturnCorrectOrganizationDetailsForValidId()
		{
			var result = await organizationService.GetOrganizationInfoAsync(MainOrganization.Id);

			Assert.IsNotNull(result);
			Assert.That(result.Name, Is.EqualTo(MainOrganization.Name));
			Assert.That(result.Id, Is.EqualTo(MainOrganization.Id));
			Assert.That(result.NumberOfAdministrators, Is.EqualTo(MainOrganization.Users.Count()));
		}

		[Test]
		public void GetOrganizationInfo_ShouldThrowExceptionIfOrganizationDoesntExist()
		{
			Assert.That(async () => await organizationService.GetOrganizationInfoAsync(Guid.NewGuid()),
				Throws.Exception.TypeOf<EntityNotFoundException>());
		}

		[Test]
		public async Task OrganizationExists_ShouldReturnTrueIfOrganizationExists()
		{
			var result = await organizationService.OrganizationExistsByIdAsync(MainOrganization.Id);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task OrganizationExists_ShouldReturnFalseIfOrganizationDoesntExist()
		{
			var result = await organizationService.OrganizationExistsByIdAsync(Guid.NewGuid());

			Assert.IsFalse(result);
		}

		[Test]
		public async Task GetUserOrganizationId_ShouldReturnCorrectOrganizatioIdForValidUser()
		{
			var result = await organizationService.GetUserOrganizationIdAsync(MainUser.Id);

			Assert.That(result, Is.EqualTo(MainOrganization.Id));
		}

		[Test]
		public void GetUserOrganizationId_ShouldThrowExceptionIfUserDoesntExist()
		{
			Assert.That(async () => await organizationService.GetUserOrganizationIdAsync("InvalidUserId"),
				Throws.Exception.TypeOf<EntityNotFoundException>());
		}

		[Test]
		public async Task GetAllUsers_ShouldReturnAllTheUsersFromTheOrganizationBasedOnTheSearchCriteria()
		{
			var result = await organizationService.GetAllUsersAsync(MainOrganization.Id, "main");

			Assert.IsNotNull(result);
			Assert.That(result.TotalCount, Is.EqualTo(1));
			Assert.That(result.Entities.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task GetAllUsers_ShouldReturnZeroResultsIfTheSearchCriteriaIsNotForAnyoneInTheOrganization()
		{
			var result = await organizationService.GetAllUsersAsync(MainOrganization.Id, "another");

			Assert.IsNotNull(result);
			Assert.That(result.TotalCount, Is.EqualTo(0));
			Assert.That(result.Entities.Count, Is.EqualTo(0));
		}

		[Test]
		public async Task CanUserAddUsers_ShouldReturnCorrectBooleanWhenTheUserExists()
		{
			var result = await organizationService.CanUserAddUsersAsync(SecondaryUserWhoCannotAddUsers.Id);

			Assert.That(result, Is.EqualTo(SecondaryUserWhoCannotAddUsers.CanAddUsers));
		}

		[Test]
		public async Task CanUserAddUsers_ShouldReturnFalseWhenTheUserDoesntExist()
		{
			var result = await organizationService.CanUserAddUsersAsync("InvalidUserId");

			Assert.False(result);
		}
	}
}
