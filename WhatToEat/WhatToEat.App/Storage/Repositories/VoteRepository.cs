using Microsoft.EntityFrameworkCore;
using WhatToEat.App.Common;
using WhatToEat.App.Storage.Model;

namespace WhatToEat.App.Storage.Repositories
{
	public class VoteRepository : RepositoryBase<Vote>
	{
		public VoteRepository(StorageContext context): base(context) { }

		public async Task<Vote> CreateAsync(User user, List<Restaurant> restaurants, CancellationToken cancellationToken)
		{
			var restaurant = new Vote { 
				Date = DateTime.UtcNow,
				User = user,
				Restaurants = restaurants,
            };
			await AddAsync(restaurant, cancellationToken);

			return restaurant;
		}

		public async Task<List<Vote>> GetAllAsync(CancellationToken cancellationToken)
		{
			return await Context.Votes.ToListAsync(cancellationToken);
		}
	}
}
