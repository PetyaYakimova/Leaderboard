namespace Leaderboard.Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        private const string DefaultMessage = "This entity could not be found!";

        public EntityNotFoundException() : base(DefaultMessage) { }

        public EntityNotFoundException(string message) : base(message) { }
    }
}
