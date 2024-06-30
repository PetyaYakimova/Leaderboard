using Leaderboard.Core.Models.Contest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Leaderboard.Controllers
{
    [Authorize]
    public class ContestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Results(Guid contestId)
        {
            return View(new ContestResultsViewModel());
        }
    }
}
