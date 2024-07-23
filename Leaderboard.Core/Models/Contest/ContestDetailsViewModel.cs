namespace Leaderboard.Core.Models.Contest
{
	/// <summary>
	/// View model only for previewing contests in details. No added validation attributes.
	/// </summary>
	public class ContestDetailsViewModel : ContestTableViewModel
	{
		public string? Description { get; set; }

		public IEnumerable<TeamResultTableViewModel> Teams { get; set; } = new List<TeamResultTableViewModel>();
	}
}
