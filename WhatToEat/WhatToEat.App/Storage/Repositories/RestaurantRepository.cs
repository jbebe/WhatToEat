using WhatToEat.App.Common;
using WhatToEat.App.Storage.Model;

namespace WhatToEat.App.Storage.Repositories
{
	public class RestaurantRepository: RepositoryBase<Restaurant>
	{
		public RestaurantRepository(StorageContext context): base(context) { }

		public async Task<Restaurant> CreateAsync(string name, IEnumerable<PaymentMethod> paymentMethods, CancellationToken cancellationToken)
		{
			var restaurant = new Restaurant { 
				Id = ModelHelpers.GenerateId(), 
				Name = name,
				PaymentMethods = paymentMethods.ToList()
			};
			await CreateOrUpdateAsync(restaurant, x => x.Id == restaurant.Id, null, cancellationToken);

			return restaurant;
		}
	}
}
