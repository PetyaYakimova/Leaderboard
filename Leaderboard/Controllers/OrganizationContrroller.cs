using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Leaderboard.Controllers
{
    [Authorize]
    public class OrganizationContrroller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
