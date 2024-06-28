﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Leaderboard.Infrastructure.Constants.DataConstants;

namespace Leaderboard.Infrastructure.Data.Models
{
	[Comment("Contests")]
	public class Contest
	{
		[Key]
		[Comment("Contest Identifier")]
		public Guid Id { get; set; }

		[Required]
		[MaxLength(ContestNameMaxLength)]
		[Comment("Contest Name")]
		public string Name { get; set; } = string.Empty;

		[Required]
		[Comment("Organization Identifier")]
		public Guid OrganizationId { get; set; }

		[ForeignKey(nameof(OrganizationId))]
		public Organization Organization { get; set; } = null!;

		public IEnumerable<Team> Teams { get; set; } = new List<Team>();
	}
}
