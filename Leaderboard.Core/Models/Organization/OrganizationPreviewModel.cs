namespace Leaderboard.Core.Models.Organization
{
    /// <summary>
    /// View model only for displaying the information for the organization. No added validations.
    /// </summary>
    public class OrganizationPreviewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int NumberOfContests { get; set; }
        
        public int NumberOfAdministrators { get; set; }
    }
}
