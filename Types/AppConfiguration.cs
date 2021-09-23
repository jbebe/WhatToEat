using Microsoft.Extensions.Configuration;
using WhatToEat.Types.Enums;

namespace WhatToEat.Types
{
  public class AppConfiguration
  {
    public class AppConstants
    {
      public AppEnvironment Environment { get; set; }

      public string AdUserInfoPath { get; set; }

      public int PollIntervalSec { get; set; }

      public int PresenceExpirationMultiplier { get; set; }

      public string LocalStorageUserDataKey { get; set; }

      public int VoteResultLimit { get; set; }
    }

    public class AppSecrets
    {
      public string StorageConnectionString { get; set; }
    }

    public AppConstants Constants { get; set; }

    public AppSecrets Secrets { get; set; }

    public AppConfiguration(IConfiguration configuration)
    {
      Constants = configuration.GetSection(nameof(Constants)).Get<AppConstants>();
      Secrets = configuration.GetSection(nameof(Secrets)).Get<AppSecrets>();
      var connectionString = System.Environment.GetEnvironmentVariable("CUSTOMCONNSTR_AZURE_STORAGE_CONNECTION_STRING");
      System.Console.WriteLine($"Connection string: {(connectionString == null ? "<null>" : "*****")}");
      if (connectionString != null)
        Secrets = new AppSecrets { StorageConnectionString = connectionString };
    }
  }
}
