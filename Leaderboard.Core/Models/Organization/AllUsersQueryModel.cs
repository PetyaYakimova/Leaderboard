using Leaderboard.Core.Models.BaseModels;

namespace Leaderboard.Core.Models.Organization
{
	/// <summary>
	/// A user query model used to get filtering criteria and pagination info. 
	/// No added validation attributes.
	/// </summary>
	public class AllUsersQueryModel : AllEntitiesQueryBaseModel<UserTableViewModel>
	{
	}
}
