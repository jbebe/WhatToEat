using System.Collections.Generic;
using System.Threading.Tasks;
using WhatToEat.Types.TableEntities;

namespace WhatToEat.Services.Scoped
{
  public partial class StorageService
  {
    public Task<UserData> GetUserAsync(string userId) =>
      GetEntityOrDefaultAsync<UserData>(UserData.PartitionKeyValue, userId);

    public Task CreateUserAsync(UserData userData) =>
      Table.UpsertEntityAsync(userData);

    public Task<List<UserData>> GetUsersAsync() =>
      GetEntitiesAsync<UserData>(x => x.PartitionKey == UserData.PartitionKeyValue);
  }
}
