﻿using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Infrastructure.Data.Common
{
	public class Repository : IRepository
	{
		private readonly DbContext context;

		public Repository(LeaderboardDbContext context)
		{
			this.context = context;
		}

		private DbSet<T> DbSet<T>() where T : class
		{
			return context.Set<T>();
		}

		public IQueryable<T> All<T>() where T : class
		{
			return DbSet<T>();
		}

		public IQueryable<T> AllAsReadOnly<T>() where T : class
		{
			return DbSet<T>().AsNoTracking();
		}

		public async Task AddAsync<T>(T entity) where T : class
		{
			await DbSet<T>().AddAsync(entity);
		}

		public async Task<int> SaveChangesAsync()
		{
			return await context.SaveChangesAsync();
		}
	}
}