using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using WhatToEat.Types.TableEntities;

namespace WhatToEat.Services.Scoped
{
  public class StorageService
  {
    private List<RestaurantData> Restaurants { get; set; }

    private TableServiceClient Client { get; }

    private TableClient Table { get; }

    public StorageService(IConfiguration config)
    {
      Client = new TableServiceClient(config.GetValue<string>("AppConfig:StorageConnectionString"));
      Client.CreateTableIfNotExists("whattoeat");
      Table = Client.GetTableClient("whattoeat");
    }

    public static string GetStorageDateString(DateTime? date = null) =>
      (date ?? DateTime.UtcNow).Date.Ticks.ToString("D");

    private async Task<T> GetEntityAsync<T>(string partitionKey, string rowKey) where T: class, ITableEntity, new()
    {
      try
      {
        var response = await Table.GetEntityAsync<T>(partitionKey, rowKey);
        return response;
      }
      catch (Azure.RequestFailedException)
      {
        return null;
      }
    }

    private async Task<List<T>> GetEntitiesAsync<T>(Expression<Func<T, bool>> filter) where T : class, ITableEntity, new()
    {
      try
      {
        var result = new List<T>();
        var entities = Table.QueryAsync(filter);
        await foreach (var entity in entities)
          result.Add(entity);

        return result;
      }
      catch (Azure.RequestFailedException)
      {
        return new List<T>();
      }
    }

    public async Task CreateRestaurantAsync(RestaurantData restaurant) =>
      await Table.UpsertEntityAsync(restaurant, TableUpdateMode.Replace);

    public async Task<List<RestaurantData>> GetRestaurantsAsync(bool forceReload = false)
    {
      if (Restaurants == null || forceReload)
      {
        var result = new List<RestaurantData>();
        var restaurants = Table.QueryAsync<RestaurantData>(r => r.PartitionKey == RestaurantData.PrimaryKey);
        await foreach (var r in restaurants)
          result.Add(r);
        Restaurants = result.OrderBy(x => x.Name).ToList();
      }

      return Restaurants;
    }

    public async Task UpsertSelectionAsync(string userId, IEnumerable<string> restaurantIds)
    {
      var userChoice = new UserChoice(userId, restaurantIds);
      await Table.UpsertEntityAsync(userChoice, TableUpdateMode.Replace);
    }

    public async Task DeleteSelectionAsync(string userId) =>
      await Table.DeleteEntityAsync(UserChoice.GetPartitionKey(DateTime.UtcNow), userId);

    public async Task<UserChoice> GetSelectionAsync(string userId) =>
      (await GetEntityAsync<UserChoice>(UserChoice.GetPartitionKey(DateTime.UtcNow), userId))
      ?? new UserChoice(userId, new List<string>());

    public async Task<List<UserChoice>> GetSelectionsAsync(string timestamp = null)
    {
      var result = await GetEntitiesAsync<UserChoice>(c =>
        (timestamp == null && (
          c.PartitionKey.CompareTo("choice_") >= 0 &&
          c.PartitionKey.CompareTo("choice_z") < 0
        ))
        || (timestamp != null && c.PartitionKey == $"choice_{timestamp}")
      );
      return (result ?? new List<UserChoice>()).ToList();
    }

    public async Task<UserData> GetUserAsync(string userId) =>
      await GetEntityAsync<UserData>(UserData.PartitionKeyValue, userId);

    public async Task CreateUserAsync(UserData userData) =>
      await Table.UpsertEntityAsync(userData);

    public async Task<List<UserData>> GetUsersAsync() =>
      await GetEntitiesAsync<UserData>(x => x.PartitionKey == UserData.PartitionKeyValue);
  }
}
