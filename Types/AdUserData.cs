using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using WhatToEat.Helpers;

namespace WhatToEat.Types
{
  public class AdUserData
  {
    public class UserClaim
    {
      [JsonPropertyName("typ")]
      public string Typ { get; set; }

      [JsonPropertyName("val")]
      public string Val { get; set; }
    }

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

    public string GetUserId() => UserClaims
      .Single(x => x.Typ.EndsWith("/emailaddress"))
      .Val.ToLowerMd5Hash();

    public string GetFullName() => UserClaims
      .Single(x => x.Typ == "name")
      .Val;
  }
}
