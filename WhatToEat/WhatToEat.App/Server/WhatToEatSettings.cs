namespace WhatToEat.App.Server;

public record DatabaseSettings(string ConnectionString);

public class WhatToEatSettings
{
#pragma warning disable CS8618
    public DatabaseSettings SQLite { get; set; }
#pragma warning restore CS8618
}
