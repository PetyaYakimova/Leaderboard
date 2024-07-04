using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Leaderboard.Controllers
{
	[Authorize]
	public class BaseController : Controller
	{
	}
}
