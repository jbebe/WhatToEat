using Azure;
using Azure.Data.Tables;
using System;

namespace WhatToEat.Types.TableEntities
{
  public class TableEntityBase : ITableEntity
  {
    public string PartitionKey { get; set; }
    
    public string RowKey { get; set; }
    
    public DateTimeOffset? Timestamp { get; set; }
    
    public ETag ETag { get; set; }

    public TableEntityBase()
    {
      Timestamp = DateTimeOffset.UtcNow;
      ETag = ETag.All;
    }

    public TableEntityBase(string partitionKey, string rowKey)
    {
      PartitionKey = partitionKey;
      RowKey = rowKey;

      Timestamp = DateTimeOffset.UtcNow;
      ETag = ETag.All;
    }
  }
}
