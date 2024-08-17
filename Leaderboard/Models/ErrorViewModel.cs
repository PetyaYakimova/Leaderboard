namespace Leaderboard.Models
{
	/// <summary>
	/// An error view model. 
	/// No added validation attributes.
	/// </summary>
	public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}