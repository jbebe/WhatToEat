namespace WhatToEat.App.Server;

public class WhatToEatConfiguration
{
    public string SQLiteConnectionString { get; set; } = default!;

    public int VoteResultLimit { get; set; }

    public int PresencePollSec { get; set; }

    public int PresenceTimeoutSec { get; set; }
}

public class WhatToEatSettings
{
#pragma warning disable CS8618
    public WhatToEatConfiguration Configuration { get; set; }
#pragma warning restore CS8618
}
