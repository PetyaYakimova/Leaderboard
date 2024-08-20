using System.ComponentModel.DataAnnotations;
using static Leaderboard.Infrastructure.Constants.DataConstants;
using static Leaderboard.Core.Constants.MessagesConstants;

namespace Leaderboard.Core.Models.Contest
{
	/// <summary>
	/// A view model for the form when editing and creating a contest. 
	/// Added validation attributes.
	/// </summary>
	public class ContestFormViewModel
	{
		[Required(ErrorMessage = RequieredMessage)]
		[StringLength(ContestNameMaxLength,
			MinimumLength = ContestNameMinLength,
			ErrorMessage = StringLengthBetweenValuesMessage)]
		public string Name { get; set; } = string.Empty;

		[StringLength(ContestDescriptionMaxLength,
			ErrorMessage = StringLengthNoMoreThanValueMessage)]
		public string? Description { get; set; }

		[Required(ErrorMessage = RequieredMessage)]
		[Display(Name = "Active")]
		public bool IsActive { get; set; }
	}
}
