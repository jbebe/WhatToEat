using Azure.Data.Tables;
using WhatToEat.Types;

namespace WhatToEat.Services.Scoped
{
  public partial class StorageService
  {
    private const string TableName = "whattoeat";

    private TableServiceClient Client { get; }

    private TableClient Table { get; }

    public StorageService(AppConfiguration config)
    {
      Client = new TableServiceClient(config.Secrets.StorageConnectionString);
      Client.CreateTableIfNotExists(TableName);
      Table = Client.GetTableClient(TableName);
    }
  }
}
