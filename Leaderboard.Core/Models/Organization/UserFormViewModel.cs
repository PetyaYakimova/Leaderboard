using System.ComponentModel.DataAnnotations;
using static Leaderboard.Infrastructure.Constants.DataConstants;
using static Leaderboard.Core.Constants.MessagesConstants;

namespace Leaderboard.Core.Models.Organization
{
	/// <summary>
	/// A view model for the form when creating users. Added validation attributes.
	/// </summary>
	public class UserFormViewModel
	{
		[Required(ErrorMessage = RequieredMessage)]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = RequieredMessage)]
		[StringLength(UserPasswordMaxLength,
				MinimumLength = UserPasswordMinLength,
				ErrorMessage = StringLengthBetweenValuesMessage)]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessage = RequieredMessage)]
		[Display(Name = "Can user add other users?")]
		public bool CanAddUsers { get; set; }
	}
}
