namespace Leaderboard.Core.Models.Contest
{
	/// <summary>
	/// View model only for previewing team before deletion. 
	/// No added validation attributes.
	/// </summary>
	public class TeamForDeleteViewModel
	{
		public Guid Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public Guid ContestId { get; set; }
	}
}
