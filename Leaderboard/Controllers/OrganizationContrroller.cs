using Leaderboard.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Leaderboard.Controllers
{
    public class OrganizationContrroller : BaseController
    {
        private readonly ILogger<OrganizationContrroller> logger;
        private readonly IOrganizationService organizationService;

        public OrganizationContrroller(ILogger<OrganizationContrroller> logger, IOrganizationService organizationService)
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
    }
}
