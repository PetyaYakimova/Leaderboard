namespace Leaderboard.Core.Models.Contest
{
	/// <summary>
	/// View model only for previewing team total result in a table. No added validation attributes.
	/// </summary>
	public class TeamResultTableViewModel
	{
		public Guid Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public bool IsActive { get; set; }

		public int TotalPoints { get; set; }
	}
}
