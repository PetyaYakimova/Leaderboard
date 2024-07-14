namespace Leaderboard.Core.Models.Contest
{
    /// <summary>
	/// View model only for previewing contests in a table. No added validation attributes.
	/// </summary>
    public class ContestTableViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public int NumberOfTeams { get; set; }
    }
}
