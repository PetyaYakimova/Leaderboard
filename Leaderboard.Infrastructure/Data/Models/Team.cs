using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Leaderboard.Infrastructure.Constants.DataConstants;

namespace Leaderboard.Infrastructure.Data.Models
{
	[Comment("Teams")]
	public class Team
	{
		[Key]
		[Comment("Team Identifier")]
		public Guid Id { get; set; }

		[Required]
		[MaxLength(TeamNameMaxLength)]
		[Comment("Team Name")]
		public string Name { get; set; } = string.Empty;

		[MaxLength(TeamNotesMaxLength)]
		[Comment("Team Notes")]
		public string? Notes { get; set; }

		[Comment("Number Of Members In The Team")]
		public int? NumberOfMembers { get; set; }

		[Required]
		[Comment("Contest Identifier")]
		public Guid ContestId { get; set; }

		[ForeignKey(nameof(ContestId))]
		public Contest Contest { get; set; } = null!;

		public IEnumerable<Point> Points { get; set; } = new List<Point>();
	}
}
