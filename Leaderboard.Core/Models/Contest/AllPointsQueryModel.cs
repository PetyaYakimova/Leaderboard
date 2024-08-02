using Leaderboard.Core.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace Leaderboard.Core.Models.Contest
{
	public class AllPointsQueryModel : AllEntitiesQueryBaseModel<PointTableViewModel>
	{
		[Display(Name = "Search by points")]
		public int? SearchPoints { get; init; }

		[Display(Name = "Search by user email")]
		public string? SearchUserEmail { get; init; }

		public string TeamName { get; set; } = string.Empty;
	}
}
