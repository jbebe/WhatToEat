using Azure.Data.Tables;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhatToEat.Types.TableEntities;

namespace WhatToEat.Services.Scoped
{
  public partial class StorageService
  {
    public Task CreateRestaurantAsync(RestaurantData restaurant) =>
      Table.UpsertEntityAsync(restaurant, TableUpdateMode.Replace);

    public Task<List<RestaurantData>> GetRestaurantsAsync() =>
      GetEntitiesAsync<RestaurantData>(restaurant => restaurant.PartitionKey == RestaurantData.PartitionKeyValue);
  }
}
