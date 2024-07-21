using Leaderboard.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Leaderboard.Attributes
{
	public class ContestExistsForTheUserOrganizationAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);

			IContestService? contestService = context.HttpContext.RequestServices.GetService<IContestService>();
			IOrganizationService? organizationService = context.HttpContext.RequestServices.GetService<IOrganizationService>();

			if (contestService == null || organizationService == null)
			{
				context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}

			object? value = context.HttpContext.GetRouteData().Values["id"];
			if (value == null)
			{
				context.Result = new StatusCodeResult(StatusCodes.Status404NotFound);
			}

			if (value != null)
			{
				Guid id = new Guid();
				if (Guid.TryParse(value.ToString(), out id))
				{
					Guid organizationId = organizationService.GetUserOrganizationIdAsync(context.HttpContext.User.Id()).Result;

					if (contestService != null && contestService.ContestExistsForOrganizationsByIdAsync(id, organizationId).Result == false)
					{
						context.Result = new StatusCodeResult(StatusCodes.Status404NotFound);
					}
				}
				else
				{
					context.Result = new StatusCodeResult(StatusCodes.Status404NotFound);
				}
			}
		}
	}
}
