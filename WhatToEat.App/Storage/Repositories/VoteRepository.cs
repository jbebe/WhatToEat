using WhatToEat.App.Common;
using WhatToEat.App.Storage.Model;

namespace WhatToEat.App.Storage.Repositories
{
    public class VoteRepository : RepositoryBase<Vote>
	{
		public VoteRepository(StorageContext context): base(context) { }

		/// <summary>
		/// Upserts a vote for the current day
		/// </summary>
		public async Task<Vote> CreateOrUpdateAsync(Id<User> userId, List<Restaurant> restaurants, CancellationToken? cancellationToken = null, DateTime? date = null)
		{
			var vote = new Vote { 
				Date = date ?? DateTime.UtcNow.Date,
				UserId = userId.Value,
				Restaurants = restaurants,
            };
			await CreateOrUpdateAsync(vote, x => x.UserId == vote.UserId && x.Date == vote.Date, x =>
			{
				x.Restaurants = vote.Restaurants;
			}, cancellationToken);

			return vote;
		}

		/// <summary>
		/// Gets the user's vote for the current day (or null)
		/// </summary>
		public async Task<Vote?> GetAsync(User user, CancellationToken cancellationToken)
		{
			var today = DateTime.UtcNow.Date;
			var votes = await GetAsync(x => x.Date == today && x.User == user, null, cancellationToken);

			return votes;
		}

	}
}
