namespace Leaderboard.Core.Exceptions
{
    public class InvalidModelException : Exception
    {
        private const string DefaultMessage = "The given model is invalid!";

        public InvalidModelException() : base(DefaultMessage) { }

        public InvalidModelException(string message) : base(message) { }
    }
}
