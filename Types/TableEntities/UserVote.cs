using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WhatToEat.Types.TableEntities
{
  public class UserVote: TableEntityBase
  {
    public const string PartitionKeyPrefix = "choice_";

    public string Choices { get; set; }

    public UserVote()
    {
    }

    public UserVote(string userId, IEnumerable<string> choiceIds): base(GetPartitionKey(DateTime.UtcNow), userId)
    {
      Choices = JsonSerializer.Serialize(choiceIds);
    }

    public string GetDate() => PartitionKey.Substring(PartitionKeyPrefix.Length);

    public string GetUserId() => RowKey;

    public List<string> GetChoicesTyped() => JsonSerializer.Deserialize<List<string>>(Choices);

    public static string GetPartitionKey(DateTime day) => $"{PartitionKeyPrefix}{day.Date.Ticks}";
  }
}
