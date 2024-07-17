namespace Leaderboard.Core.Exceptions
{
	public class CannotDeleteEntityException : Exception
	{
		private const string DefaultMessage = "This entity could not be deleted!";

		public CannotDeleteEntityException() : base(DefaultMessage) { }

		public CannotDeleteEntityException(string message) : base(message) { }
	}
}
