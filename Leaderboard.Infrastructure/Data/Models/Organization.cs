﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Leaderboard.Infrastructure.Constants.DataConstants;

namespace Leaderboard.Infrastructure.Data.Models
{
	[Comment("Organizations")]
	public class Organization
	{
		[Key]
		[Comment("Organization Identifier")]
		public Guid Id { get; set; }

		[Required]
		[MaxLength(OrganizationNameMaxLength)]
		[Comment("Organization Name")]
		public string Name { get; set; } = string.Empty;

		public IEnumerable<Contest> Contests { get; set; } = new List<Contest>();

		public IEnumerable<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
	}
}
