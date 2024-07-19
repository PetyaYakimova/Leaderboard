using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Leaderboard.Infrastructure.Constants.DataConstants;

namespace Leaderboard.Infrastructure.Data.Models
{
	[Comment("Point For All Teams")]
	public class Point
	{
		[Key]
		[Comment("Point Identifier")]
		public int Id { get; set; }

		[Required]
		[Comment("Team Identifier")]
		public Guid TeamId { get; set; }

		[ForeignKey(nameof(TeamId))]
		public Team Team { get; set; } = null!;

		[Required]
		[Comment("Team Points")]
		public int Points { get; set; }

		[MaxLength(PointsDescriptionMaxLength)]
		[Comment("Description For Giving Points")]
		public string? Description { get; set; }

		public string AddedByUserId { get; set; } = string.Empty;

		[ForeignKey(nameof(AddedByUserId))]
		public ApplicationUser AddedByUser { get; set; } = null!;
	}
}
