using Leaderboard.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Leaderboard.Attributes
{
	public class UserCanAddUsersAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);

			IOrganizationService? service = context.HttpContext.RequestServices.GetService<IOrganizationService>();

			if (service == null)
			{
				context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}

			var userId = context.HttpContext.User.Id();

			if (service.CanUserAddUsersAsync(userId).Result == false)
			{
				context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
			}
		}
	}
}
