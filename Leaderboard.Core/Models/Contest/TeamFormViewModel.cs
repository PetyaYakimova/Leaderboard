using System.ComponentModel.DataAnnotations;
using static Leaderboard.Infrastructure.Constants.DataConstants;
using static Leaderboard.Core.Constants.MessagesConstants;

namespace Leaderboard.Core.Models.Contest
{
	/// <summary>
	/// A view model for the form when editing and creating a team. Added validation attributes.
	/// </summary>
	public class TeamFormViewModel
	{
		[Required(ErrorMessage = RequieredMessage)]
		[StringLength(TeamNameMaxLength,
			MinimumLength = TeamNameMinLength,
			ErrorMessage = StringLengthBetweenValuesMessage)]
		public string Name { get; set; } = string.Empty;

		[StringLength(TeamNotesMaxLength,
			ErrorMessage = StringLengthNoMoreThanValueMessage)]
		public string? Notes { get; set; }

		[Range(TeamNumberOfMembersMin,
			TeamNumberOfMembersMax,
			ErrorMessage = NumberMustBeInRangeErrorMessage)]
		[Display(Name = "Number of members")]
		public int? NumberOfMembers { get; set; }

		[Required(ErrorMessage = RequieredMessage)]
		[Display(Name = "Active")]
		public bool IsActive { get; set; }
	}
}
