using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WhatToEat.Services.Scoped
{
  public partial class StorageService
  {
    private static string GetStorageDateString(DateTime date) => date.Date.Ticks.ToString("D");

    private async Task<T> GetEntityOrDefaultAsync<T>(string partitionKey, string rowKey) where T : class, ITableEntity, new()
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
  }
}
