namespace WhatToEat.App.Server;

public record WhatToEatConfiguration(
    string SQLiteConnectionString,
	int VoteResultLimit
);

public class WhatToEatSettings
{
#pragma warning disable CS8618
    public WhatToEatConfiguration Configuration { get; set; }
#pragma warning restore CS8618
}
