using Leaderboard.Attributes;
using Leaderboard.Core.Contracts;
using Leaderboard.Core.Models.Organization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Leaderboard.Controllers
{
	public class OrganizationController : BaseController
	{
		private readonly ILogger<OrganizationController> logger;
		private readonly IOrganizationService organizationService;

		public OrganizationController(ILogger<OrganizationController> logger, IOrganizationService organizationService)
		{
			this.logger = logger;
			this.organizationService = organizationService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			Guid organizationId = await organizationService.GetUserOrganizationIdAsync(User.Id());
			var model = await organizationService.GetOrganizationInfoAsync(organizationId);

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Administrators([FromQuery] AllUsersQueryModel query)
		{
			Guid organizationId = await organizationService.GetUserOrganizationIdAsync(User.Id());
			var model = await organizationService.GetAllUsersAsync(
				organizationId,
				query.SearchTerm,
				query.CurrentPage,
				query.ItemsPerPage);

			query.TotalItemCount = model.TotalCount;
			query.Entities = model.Entities;

			return View(query);
		}

		[HttpGet]
		[UserCanAddUsers]
		public IActionResult AddAdministrator()
		{
			UserFormViewModel model = new UserFormViewModel();

			return View(model);
		}

		[HttpPost]
		[UserCanAddUsers]
		public async Task<IActionResult> AddAdministrator(UserFormViewModel model)
		{
			if (ModelState.IsValid == false)
			{
				return View(model);
			}

			Guid organizationId = await organizationService.GetUserOrganizationIdAsync(User.Id());

			await organizationService.AddUserAsync(model, organizationId);

			return RedirectToAction(nameof(Administrators));
		}
	}
}
