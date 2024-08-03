namespace Leaderboard.Core.Models.Contest
{
	/// <summary>
	/// A view model only for displaying the pinned contests and adding option to pin unpinned contest. No added validation attributes.
	/// </summary>
	public class PinnedContestsViewModel
	{
		public IEnumerable<ContestTableViewModel> PinnedContests { get; set; } = new List<ContestTableViewModel>();

		public IEnumerable<ContestForPreviewViewModel> UnpinnedActiveContests { get; set; } = new List<ContestForPreviewViewModel>();
	}
}
