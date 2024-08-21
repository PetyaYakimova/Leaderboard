using Leaderboard.Core.Models.BaseModels;

namespace Leaderboard.Core.Models.Contest
{
	/// <summary>
	/// A model that has the total count of points records and a collection of a certain amount of them to display them on pages. 
	/// No added validation attributes.
	/// </summary>
	public class PointsQueryServiceModel : EntityQueryServiceBaseModel<PointTableViewModel>
	{
	}
}
