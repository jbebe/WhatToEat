using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhatToEat.Types.TableEntities;

namespace WhatToEat.Services.Scoped
{
  public partial class StorageService
  {
    public async Task UpsertVoteAsync(string userId, IEnumerable<string> restaurantIds)
    {
      var userChoice = new UserVote(userId, restaurantIds);
      await Table.UpsertEntityAsync(userChoice, TableUpdateMode.Replace);
    }

    public Task DeleteVoteAsync(string userId) =>
      Table.DeleteEntityAsync(UserVote.GetPartitionKey(DateTime.UtcNow), userId);

    public async Task<UserVote> GetVoteAsync(string userId) =>
      (await GetEntityOrDefaultAsync<UserVote>(UserVote.GetPartitionKey(DateTime.UtcNow), userId))
      ?? new UserVote(userId, new List<string>());

    public async Task<List<UserVote>> GetVotesAsync(DateTime? day = null)
    {
      List<UserVote> result;
      var queryAllVotes = day == null;
      if (queryAllVotes)
      {
        result = await GetEntitiesAsync<UserVote>(c =>
          c.PartitionKey.CompareTo("choice_") >= 0 &&
          c.PartitionKey.CompareTo("choice_z") < 0);
      } 
      else
      {
        var dateString = GetStorageDateString(day.Value);
        result = await GetEntitiesAsync<UserVote>(c =>
          dateString != null && c.PartitionKey == $"choice_{dateString}");
      }

      return result;
    }
  }
}
