namespace Leaderboard.Core.Models.Contest
{
	/// <summary>
	/// View model only for previewing contests in a table or when deleting them.
	/// No added validation attributes.
	/// </summary>
	public class ContestLeaderboardViewModel
	{
		public string Name { get; set; } = string.Empty;

		public int NumberOfTeams { get; set; }

		public string? Description { get; set; }

		public IEnumerable<TeamLeaderboardTableViewModel> Teams { get; set; } = new List<TeamLeaderboardTableViewModel>();
	}
}
