using System.Timers;
using WhatToEat.App.Services.Models;
using WhatToEat.App.Server;
using WhatToEat.App.Storage.Model;
using WhatToEat.App.Common;
using WhatToEat.App.Storage.Repositories;
using System.Collections.Immutable;

namespace WhatToEat.App.Services;

public sealed class PresenceService : IDisposable
{
    public Dictionary<Id<User>, User> Users { get; set; }

	public IReadOnlyList<User> OnlineUsers
	{
		get
		{
            Console.WriteLine("Users: " + string.Join(", ", UserPresence.Keys.Select(x => Users[x].Name)));
            return UserPresence.Keys.Select(x => Users[x]).ToImmutableList();
		}
	}

	Dictionary<Id<User>, int> UserPresence { get; } = new();

    UserRepository UserRepository { get; set; }

    SessionService SessionService { get; }

    WhatToEatSettings Settings { get; set; }

    GlobalEventService GlobalEventService { get; set; }

	LocalEventService LocalEventService { get; set; }

    System.Timers.Timer? Timer { get; set; }
    
    public event Action? OnPresenceChanged;

    public PresenceService(
        UserRepository userRepository,
        SessionService sessionService,
		WhatToEatSettings settings,
		GlobalEventService globalEventService,
		LocalEventService localEventService)
	{
        Users = new();
        UserRepository = userRepository;
        SessionService = sessionService;
        Settings = settings;
        GlobalEventService = globalEventService;
		LocalEventService = localEventService;
        
        GlobalEventService.OnMessage -= OnMessage;
        GlobalEventService.OnMessage += OnMessage;
		LocalEventService.OnMessage -= OnMessage;
		LocalEventService.OnMessage += OnMessage;
	}

	private async Task OnMessage(BroadcastMessage message)
	{
		if (message.Type == BroadcastEventType.LoggedIn)
		{
            // Start timer
            if (Timer != null) throw new Exception("This event cannot appear multiple times");
            Timer = new(TimeSpan.FromSeconds(Settings.Configuration.PresencePollSec).TotalMilliseconds);
            Timer.Elapsed -= RemotePresenceUpdater;
            Timer.Elapsed += RemotePresenceUpdater;
            Timer.Start();

            // Load users to cache
            Users = (await UserRepository.GetAllAsync(null, CancellationToken.None)).ToDictionary(x => x.IdTyped);
		}
        else if (message is PresenceChanged presence)
        {
            // Send event if someone new showed up
            var isNewUser = !UserPresence.ContainsKey(presence.UserId);
            UserPresence[presence.UserId] = Settings.Configuration.PresenceTimeoutSec;
            if (isNewUser) OnPresenceChanged?.Invoke();
        }
	}

    private void RemotePresenceUpdater(object? sender, ElapsedEventArgs e)
    {
        // Update presence of current user
        UserPresence[SessionService.User!.IdTyped] = Settings.Configuration.PresenceTimeoutSec;

        // Decrement presence counter of all users
        var removable = new List<Id<User>>();
        foreach (var userId in UserPresence.Keys)
        {
            UserPresence[userId] -= 1;
            if (UserPresence[userId] <= 0)
                removable.Add(userId);
        }

        foreach (var userId in removable)
            UserPresence.Remove(userId);

        // Send event if someone has timed out
        if (removable.Any())
            OnPresenceChanged?.Invoke();
    }

    public void Dispose()
    {
        Timer?.Stop();
		LocalEventService.OnMessage -= OnMessage;
		GlobalEventService.OnMessage -= OnMessage;
    }
}
