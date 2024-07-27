namespace Leaderboard.Infrastructure.Constants
{
	public static class DataConstants
	{
		public const int OrganizatioNameMinLength = 3;
		public const int OrganizationNameMaxLength = 40;

		public const int ContestNameMinLength = 3;
		public const int ContestNameMaxLength = 40;

		public const int ContestDescriptionMaxLength = 150;

		public const int TeamNameMinLength = 2;
		public const int TeamNameMaxLength = 60;

		public const int TeamNotesMaxLength = 150;

		public const int TeamNumberOfMembersMin = 1;
		public const int TeamNumberOfMembersMax = 100;

		public const int PointsDescriptionMaxLength = 150;

		public const int PointsMin = -500;
		public const int PointsMax = 500;

		public const int UserPasswordMinLength = 8;
		public const int UserPasswordMaxLength = 100;
	}
}
