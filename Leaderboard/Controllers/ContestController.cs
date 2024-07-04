using Leaderboard.Attributes;
using Leaderboard.Core.Contracts;
using Leaderboard.Core.Models.Contest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Leaderboard.Controllers
{
	public class ContestController : BaseController
	{
		private readonly ILogger<ContestController> logger;
		private readonly IOrganizationService organizationService;
		private readonly IContestService contestService;

		public ContestController
			(ILogger<ContestController> logger, 
			IOrganizationService organizationService,
			IContestService contestService)
		{
			this.logger = logger;
			this.organizationService = organizationService;
			this.contestService = contestService;
		}

		[HttpGet]
		public async Task<IActionResult> All()
		{
			Guid organizationId = await organizationService.GetUserOrganizationIdAsync(User.Id());
			//TODO: Get the user organization id -> when you have extended the user and when you have a custom method that return
			return View();
		}

		[HttpGet]
		public IActionResult Add()
		{
			var model = new ContestFormViewModel();

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(ContestFormViewModel model)
		{
			Guid organizationId = await organizationService.GetUserOrganizationIdAsync(User.Id());

			if (ModelState.IsValid == false)
			{
				return View(model);
			}

			var contestId = await this.contestService.CreateContestAsync(model, organizationId);

			//TODO: Check in ChoreographyBuilder if this is correct
			return RedirectToAction(nameof(Details), new { id = contestId });
		}

		[HttpGet]
		[ContestExistsForTheUserOrganization]
		public async Task<IActionResult> Details(Guid id)
		{
			return View();
		}

		[HttpGet]
		[AllowAnonymous]
		[ContestExists]
		public async Task<IActionResult> Results(Guid id)
		{
			return View(new ContestResultsViewModel());
		}
	}
}
