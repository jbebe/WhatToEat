using System;
using System.Collections.Generic;
using System.Text.Json;

namespace WhatToEat.Types.TableEntities
{
  public class UserVote: TableEntityBase
  {
    public const string PartitionKeyPrefix = "vote_";

    public string Votes { get; set; }

    public UserVote()
    {
    }

    public UserVote(string userId, IEnumerable<string> voteIds): base(GetPartitionKey(DateTime.UtcNow), userId)
    {
      Votes = JsonSerializer.Serialize(voteIds);
    }

    public string GetDate() => PartitionKey.Substring(PartitionKeyPrefix.Length);

    public string GetUserId() => RowKey;

    public List<string> GetVotesTyped() => JsonSerializer.Deserialize<List<string>>(Votes);

    public static string GetPartitionKey(DateTime day) => $"{PartitionKeyPrefix}{day.Date.Ticks}";
  }
}
