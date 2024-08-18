namespace Leaderboard.Core.Models.BaseModels
{
	/// <summary>
	/// A base class for all models that get the total count of all entities and a collection of a certain amount of them to display them on pages. 
	/// No added validation attributes.
	/// </summary>
	/// <typeparam name="T">Type of model entities</typeparam>
	public class EntityQueryServiceBaseModel<T>
	{
		public int TotalCount { get; set; }

		public IEnumerable<T> Entities { get; set; } = new List<T>();
	}
}
