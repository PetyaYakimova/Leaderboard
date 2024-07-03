using Leaderboard.Core.Contracts;
using Leaderboard.Core.Models.Contest;
using Leaderboard.Infrastructure.Data.Common;
using Leaderboard.Infrastructure.Data.Models;

namespace Leaderboard.Core.Services
{
	public class ContestService : IContestService
	{
		private readonly IRepository repository;

		public ContestService(IRepository repository)
		{
			this.repository = repository;
		}

		public async Task<Guid> CreateContestAsync(ContestFormViewModel model, Guid organizationId)
		{
			//TODO: Check if organizationId exists - if not throw an error 
			Contest contest = new Contest()
			{
				Id = new Guid(),
				Name = model.Name,
				Description = model.Description,
				IsActive = model.IsActive,
				OrganizationId = organizationId
			};

			await repository.AddAsync(contest);
			await repository.SaveChangesAsync();

			return contest.Id;
		}
	}
}
