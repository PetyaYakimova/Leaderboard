namespace Leaderboard.Core.Models.Contest
{
	/// <summary>
	/// View model only for previewing team total result in a table. No added validation attributes.
	/// </summary>
	public class TeamLeaderboardTableViewModel
	{
		public string Name { get; set; } = string.Empty;

		public int TotalPoints { get; set; }
	}
}
