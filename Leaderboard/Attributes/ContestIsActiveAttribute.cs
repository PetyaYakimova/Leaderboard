using Leaderboard.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Leaderboard.Attributes
{
	public class ContestIsActiveAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);

			IContestService? contestService = context.HttpContext.RequestServices.GetService<IContestService>();

			if (contestService == null)
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
					if (contestService != null && contestService.ContestIsActiveAsync(id).Result == false)
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
