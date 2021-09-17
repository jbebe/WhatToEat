using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azure;
using Azure.Data.Tables;
using WhatToEat.Helpers;

namespace WhatToEat.Types
{
  public enum PaymentMethod
  {
    Cash     = 1 << 0,
    BankCard = 1 << 1,
    SzepCard = 1 << 2,
  }

  public static class PaymentMethodExtensions
  {
    public static List<PaymentMethod> ToValues(this PaymentMethod method)
    {
      var output = new List<PaymentMethod>();
      for (var i = 0; i < sizeof(PaymentMethod); ++i)
      {
        var enumValue = (PaymentMethod)(1 << i);
        var isSet = (method & enumValue) != 0;
        if (isSet)
          output.Add(enumValue);
      }

      return output;
    }

    public static string ToStrings(this PaymentMethod method, string separator = ", ", string format = "G") =>
      string.Join(separator, ToValues(method).Select(x => x.ToString(format)));
  }

  public class RestaurantData: TableEntityBase
  {
    public const string PrimaryKey = "restaurant";

    public string Name { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public RestaurantData(){}

    public RestaurantData(string restaurantId): base(PrimaryKey, restaurantId)
    {
    }

    public string GetId() => RowKey;
  }

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
