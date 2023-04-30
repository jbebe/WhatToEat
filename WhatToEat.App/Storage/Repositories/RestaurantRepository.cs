using WhatToEat.App.Common;
using WhatToEat.App.Storage.Model;

namespace WhatToEat.App.Storage.Repositories
{
	public class RestaurantRepository: RepositoryBase<Restaurant>
	{
		public RestaurantRepository(StorageContext context): base(context) { }

		public async Task<Restaurant> CreateAsync(string name, IEnumerable<PaymentMethod> paymentMethods, IEnumerable<ConsumptionType> consumptionTypes, CancellationToken? cancellationToken = null)
		{
			var restaurant = new Restaurant { 
				Id = new Id<Restaurant>().Value,
				Name = name,
				PaymentMethods = paymentMethods.ToList(),
				ConsumptionTypes = consumptionTypes.ToList(),
			};
			await CreateOrUpdateAsync(restaurant, x => x.Id == restaurant.Id, null, cancellationToken ?? CancellationToken.None);

			return restaurant;
		}
	}
}
