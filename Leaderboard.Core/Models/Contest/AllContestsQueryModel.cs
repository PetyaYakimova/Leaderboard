using Leaderboard.Core.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace Leaderboard.Core.Models.Contest
{
	/// <summary>
	/// A contest query model used to get filtering criteria and pagination info. 
	/// No added validation attributes.
	/// </summary>
	public class AllContestsQueryModel : AllEntitiesQueryBaseModel<ContestTableViewModel>
    {
        [Display(Name = "Number of teams")]
        public int? SearchNumberOfTeams { get; init; }
    }
}
