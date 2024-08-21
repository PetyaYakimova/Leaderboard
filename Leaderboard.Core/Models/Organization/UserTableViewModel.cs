namespace Leaderboard.Core.Models.Organization
{
	/// <summary>
	/// View model only for previewing users in a table. 
	/// No added validation attributes.
	/// </summary>
	public class UserTableViewModel
	{
		public string Email { get; set; } = string.Empty;

		public bool CanAddUsers { get; set; }
	}
}
