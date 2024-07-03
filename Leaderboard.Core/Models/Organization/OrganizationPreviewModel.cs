using System.ComponentModel.DataAnnotations;

namespace Leaderboard.Core.Models.Organization
{
    /// <summary>
    /// View model only for displaying the information for the organization. No added validation attributes.
    /// </summary>
    public class OrganizationPreviewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        [Display(Name="Number of contests:")]
        public int NumberOfContests { get; set; }

        [Display(Name="Number of administrators:")]
        public int NumberOfAdministrators { get; set; }
    }
}
