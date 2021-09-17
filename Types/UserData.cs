using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatToEat.Types
{
  public class LocalUserData
  {
    public const string Key = "AdUserInfo";

    public string UserId { get; set; }
  }

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

  public class UserClaim
  {
    [JsonPropertyName("typ")]
    public string Typ { get; set; }

    [JsonPropertyName("val")]
    public string Val { get; set; }
  }

  public class AdUserData
  {
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("expires_on")]
    public DateTime ExpiresOn { get; set; }

    [JsonPropertyName("id_token")]
    public string IdToken { get; set; }

    [JsonPropertyName("provider_name")]
    public string ProviderName { get; set; }

    [JsonPropertyName("user_claims")]
    public List<UserClaim> UserClaims { get; set; }

    [JsonPropertyName("user_id")]
    public string UserId { get; set; }
  }
}
