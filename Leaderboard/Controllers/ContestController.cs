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

		[HttpGet]
		[ContestExistsForTheUserOrganization]
		public async Task<IActionResult> Edit(Guid id)
		{
			ContestFormViewModel model = await contestService.GetContestByIdAsync(id);

			return View(model);
		}

		[HttpPost]
		[ContestExistsForTheUserOrganization]
		public async Task<IActionResult> Edit(Guid id, ContestFormViewModel model)
		{
			if (ModelState.IsValid == false)
			{
				return View(model);
			}

			await contestService.EditContestAsync(id, model);

			return RedirectToAction(nameof(All));
		}

		[HttpGet]
		[ContestExistsForTheUserOrganization]
		[ContestHasNoTeams]
		public async Task<IActionResult> Delete(Guid id)
		{
			var model = await contestService.GetContestForPreviewAsync(id);

			return View(model);
		}

		[HttpPost]
		[ContestExistsForTheUserOrganization]
		[ContestHasNoTeams]
		public async Task<IActionResult> Delete(ContestTableViewModel model)
		{
			await contestService.DeleteContestAsync(model.Id);

			return RedirectToAction(nameof(All));
		}

		[HttpGet]
        [ContestExistsForTheUserOrganization]
        public async Task<IActionResult> Details(Guid id)
        {
            var model = await contestService.GetContestDetailsAsync(id);
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [ContestExists]
        [ContestIsActive]
        public async Task<IActionResult> Leaderboard(Guid id)
        {
			var model = await contestService.GetContestLeaderboardAsync(id);
			return View(model);
		}

		//TODO: Add pin contest to start for users and unpin on the home page with cards

		[HttpGet]
		[ContestExistsForTheUserOrganization]
		[ContestIsActive]
		public IActionResult AddTeam(Guid id)
		{
			var model = new TeamFormViewModel();

			return View(model);
		}

		[HttpPost]
		[ContestExistsForTheUserOrganization]
		[ContestIsActive]
		public async Task<IActionResult> AddTeam(Guid id, TeamFormViewModel model)
		{
			if (ModelState.IsValid == false)
			{
				return View(model);
			}

			await this.contestService.CreateTeamAsync(model, id);

			return RedirectToAction(nameof(Details), new { id = id });
		}

		[HttpPost]
		[TeamExistsForTheUserOrganization]
		public async Task<IActionResult> ChangeStatusTeam(Guid id)
		{
			await contestService.ChangeTeamStatusAsync(id);

			var contestId = await contestService.GetContestForTeamByIdAsync(id);

			return RedirectToAction(nameof(Details), new { id = contestId });
		}

		[HttpGet]
		[TeamExistsForTheUserOrganization]
		public async Task<IActionResult> EditTeam(Guid id)
		{
			TeamFormViewModel model = await contestService.GetTeamByIdAsync(id);

			return View(model);
		}

		[HttpPost]
		[TeamExistsForTheUserOrganization]
		public async Task<IActionResult> EditTeam(Guid id, TeamFormViewModel model)
		{
			if (ModelState.IsValid == false)
			{
				return View(model);
			}

			await contestService.EditTeamAsync(id, model);

			var contestId = await contestService.GetContestForTeamByIdAsync(id);

			return RedirectToAction(nameof(Details), new { id = contestId });
		}

		[HttpGet]
		[TeamExistsForTheUserOrganization]
		public async Task<IActionResult> DeleteTeam(Guid id)
		{
			var model = await contestService.GetTeamForDeleteByIdAsync(id);

			return View(model);
		}

		[HttpPost]
		[TeamExistsForTheUserOrganization]
		public async Task<IActionResult> DeleteTeam(TeamResultTableViewModel model)
		{
			var contestId = await contestService.GetContestForTeamByIdAsync(model.Id);

			await contestService.DeleteTeamAsync(model.Id);

			return RedirectToAction(nameof(Details), new { id = contestId });
		}


		[HttpGet]
		[TeamExistsForTheUserOrganization]
		[TeamIsActive]
		public async Task<IActionResult> AddPoints(Guid id)
		{
			var model = new PointFormViewModel();
			model.TeamName = await contestService.GetTeamNameByIdAsync(id);

			return View(model);
		}

		[HttpPost]
		[TeamExistsForTheUserOrganization]
		[TeamIsActive]
		public async Task<IActionResult> AddPoints(Guid id, PointFormViewModel model)
		{
			var contestId = await contestService.GetContestForTeamByIdAsync(id);

			if (ModelState.IsValid == false)
			{
				model.TeamName = await contestService.GetTeamNameByIdAsync(id);
				return View(model);
			}

			await this.contestService.CreatePointAsync(model, id, User.Id());

			return RedirectToAction(nameof(Details), new { id = contestId });
		}

		//TODO: After that continue with view points history
	}
}
