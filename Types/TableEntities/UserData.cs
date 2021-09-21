using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatToEat.Types.TableEntities
{
  public class UserData: TableEntityBase
  {
    public const string PartitionKeyValue = "user";

    public string Name { get; set; }

    public UserData()
    {
    }

    public UserData(string userId, string name): base(PartitionKeyValue, userId)
    {
      Name = name;
    }

    public string GetUserId() => RowKey;
  }
}
