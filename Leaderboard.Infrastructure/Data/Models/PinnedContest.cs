using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Leaderboard.Infrastructure.Data.Models
{
	[Comment("Pinned Contests For Users")]
	public class PinnedContest
	{
		[Required]
		[Comment("Contest Identifier")]
		public Guid ContestId { get; set; }

		[ForeignKey(nameof(ContestId))]
		public Contest Contest { get; set; } = null!;

		[Required]
		[Comment("User Identifier")]
		public string UserId { get; set; } = string.Empty;

		[ForeignKey(nameof(UserId))]
		public ApplicationUser User { get; set; } = null!;
	}
}
