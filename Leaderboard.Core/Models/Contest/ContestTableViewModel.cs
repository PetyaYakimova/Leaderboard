namespace Leaderboard.Core.Models.Contest
{
    /// <summary>
	/// View model only for previewing contests in a table or when deleting them. No added validation attributes.
	/// </summary>
    public class ContestTableViewModel : ContestForPreviewViewModel
	{
        public bool IsActive { get; set; }

        public int NumberOfTeams { get; set; }
    }
}
