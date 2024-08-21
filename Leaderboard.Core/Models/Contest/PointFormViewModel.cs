using static Leaderboard.Infrastructure.Constants.DataConstants;
using static Leaderboard.Core.Constants.MessagesConstants;
using System.ComponentModel.DataAnnotations;

namespace Leaderboard.Core.Models.Contest
{
	/// <summary>
	/// A view model for the form when adding points for a team. 
	/// Added validation attributes.
	/// </summary>
	public class PointFormViewModel
	{
		[Required(ErrorMessage = RequieredMessage)]
		[Range(PointsMin,
			PointsMax,
			ErrorMessage = NumberMustBeInRangeErrorMessage)]
		public int Points { get; set; }

		[StringLength(PointsDescriptionMaxLength,
			ErrorMessage = StringLengthNoMoreThanValueMessage)]
		public string? Description { get; set; }

		public string TeamName { get; set; } = null!;
	}
}
