namespace Leaderboard.Core.Models.Contest
{
	/// <summary>
	/// View model only for previewing basic contest info. No added validation attributes.
	/// </summary>
	public class ContestForPreviewViewModel
	{
		public Guid Id { get; set; }

		public string Name { get; set; } = string.Empty;
	}
}
