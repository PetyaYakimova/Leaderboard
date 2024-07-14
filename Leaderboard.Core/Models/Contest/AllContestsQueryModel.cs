using Leaderboard.Core.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace Leaderboard.Core.Models.Contest
{
    public class AllContestsQueryModel : AllEntitiesQueryBaseModel<ContestTableViewModel>
    {
        [Display(Name = "Number of teams")]
        public int? SearchNumberOfTeams { get; init; }
    }
}
