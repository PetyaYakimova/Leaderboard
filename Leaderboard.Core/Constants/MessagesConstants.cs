namespace Leaderboard.Core.Constants
{
	public static class MessagesConstants
	{
		public const string RequieredMessage = "The {0} field is required.";

		public const string StringLengthBetweenValuesMessage = "The {0} field must be between {2} and {1} characters long.";

		public const string StringLengthNoMoreThanValueMessage = "The {0} field must be no more than {1} characters long.";

        //Logger messages
        public const string EntityWithIdWasNotFoundLoggerErrorMessage = "{0} with id {1} was not found!";
    }
}
