namespace Leaderboard.Core.Models.Contest
{
	/// <summary>
	/// View model only for previewing points for a team in a table. 
	/// No added validation attributes.
	/// </summary>
	public class PointTableViewModel
	{
		public int Points { get; set; }

		public string? Description { get; set; }

		public string AddedByUserWithEmail { get; set; } = string.Empty;
	}
}
