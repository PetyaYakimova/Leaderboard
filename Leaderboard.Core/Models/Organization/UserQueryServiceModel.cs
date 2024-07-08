using Leaderboard.Core.Models.BaseModels;

namespace Leaderboard.Core.Models.Organization
{
	/// <summary>
	/// A model that has the total count of users and a collection of a certain amount of them to display them on pages. No added validation attributes.
	/// </summary>
	public class UserQueryServiceModel : EntityQueryServiceBaseModel<UserTableViewModel>
	{
	}
}
