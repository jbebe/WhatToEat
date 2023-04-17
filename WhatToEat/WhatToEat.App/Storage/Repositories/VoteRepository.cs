using WhatToEat.App.Common;
using WhatToEat.App.Storage.Model;

namespace WhatToEat.App.Storage.Repositories
{
    public class VoteRepository : RepositoryBase<Vote>
	{
		public VoteRepository(StorageContext context): base(context) { }

		public async Task<Vote> CreateOrUpdateAsync(User user, List<Id<Restaurant>> ids, CancellationToken cancellationToken)
		{
			var vote = new Vote { 
				Date = DateTime.UtcNow.Date,
				User = user,
				UserId = user.Id,
				RestaurantIds = ids,
            };
			await CreateOrUpdateAsync(vote, x => x.UserId == vote.UserId && x.Date == vote.Date, x =>
			{
				x.RestaurantIds = vote.RestaurantIds;
			}, cancellationToken);

			return vote;
		}

		public async Task<Vote?> GetAsync(User user, CancellationToken cancellationToken)
		{
			var today = DateTime.UtcNow.Date;
			var votes = await QueryAsync(x => x.Date == today && x.User == user, cancellationToken);

			return votes.SingleOrDefault();
		}

	}
}
