using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leaderboard.Infrastructure.Data.Models
{
	public class ApplicationUser : IdentityUser
	{
		[Required]
		[Comment("Organization Identifier")]
		public Guid OrganizationId { get; set; }

		[ForeignKey(nameof(OrganizationId))]
		public Organization Organization { get; set; } = null!;

		[Required]
		[Comment("Can User Add Other Users In The Organization")]
		public bool CanAddUsers { get; set; }

		public IEnumerable<Point> AddedPoints { get; set; } = new List<Point>();
	}
}
