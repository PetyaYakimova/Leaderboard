using Leaderboard.Attributes;
using Leaderboard.Core.Contracts;
using Leaderboard.Core.Models.Contest;
using Leaderboard.Core.Models.Organization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public async Task<IActionResult> All([FromQuery] AllContestsQueryModel query)
        {
            Guid organizationId = await organizationService.GetUserOrganizationIdAsync(User.Id());
            var model = await contestService.GetAllContestsForOrganizationAsync(
                organizationId,
                query.SearchTerm,
                query.SearchNumberOfTeams,
                query.CurrentPage,
                query.ItemsPerPage);

            query.TotalItemCount = model.TotalCount;
            query.Entities = model.Entities;

            return View(query);
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

            return RedirectToAction(nameof(Details), new { id = contestId });
        }

		[HttpPost]
		[ContestExistsForTheUserOrganization]
		public async Task<IActionResult> ChangeStatus(Guid id)
		{
			await contestService.ChangeContestStatusAsync(id);

			return RedirectToAction(nameof(All));
		}

		//TODO: Continue with contest edit and delete and details

		[HttpGet]
        [ContestExistsForTheUserOrganization]
        public async Task<IActionResult> Details(Guid id)
        {
            return View();
        }

        //TOOO: After that continue with adding points, view points history and leaderboard

        [HttpGet]
        [AllowAnonymous]
        [ContestExists]
        public async Task<IActionResult> Leaderboard(Guid id)
        {
            return View(new ContestResultsViewModel());
        }
    }
}
