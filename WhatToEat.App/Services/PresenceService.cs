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

	public IReadOnlyList<User> OnlineUsers => UserPresence.Keys.Select(x => Users[x]).ToImmutableList();

	Dictionary<Id<User>, int> UserPresence { get; } = new();

    UserRepository UserRepository { get; set; }

    WhatToEatSettings Settings { get; set; }

    GlobalEventService GlobalEventService { get; set; }

    System.Timers.Timer? Timer { get; }
    
    public event Action? OnPresenceChanged;

    public PresenceService(
        UserRepository userRepository,
		WhatToEatSettings settings,
		GlobalEventService globalEventService)
	{
        Users = new();
        UserRepository = userRepository;
        Settings = settings;
        GlobalEventService = globalEventService;
        
        GlobalEventService.OnMessage -= OnMessage;
        GlobalEventService.OnMessage += OnMessage;

		// Start timer
		Timer = new(TimeSpan.FromSeconds(Settings.Configuration.PresencePollSec).TotalMilliseconds);
		Timer.Elapsed -= RemotePresenceUpdater;
		Timer.Elapsed += RemotePresenceUpdater;
		Timer.Start();

		// Load users to cache
		Users = UserRepository.GetAllAsync(null, CancellationToken.None).Result.ToDictionary(x => x.IdTyped);
	}

	private Task OnMessage(BroadcastMessage message)
	{
        if (message is PresenceChanged presence)
        {
            // Send event if someone new showed up
            var isNewUser = !UserPresence.ContainsKey(presence.UserId);
            UserPresence[presence.UserId] = Settings.Configuration.PresenceTimeoutSec;
            if (isNewUser) OnPresenceChanged?.Invoke();
        }

        return Task.CompletedTask;
	}

    private void RemotePresenceUpdater(object? sender, ElapsedEventArgs e)
    {
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

    public void UpdatePresence(Id<User> userId)
    {
		// Update presence of current user
		UserPresence[userId] = Settings.Configuration.PresenceTimeoutSec;
	}

	public void Dispose()
    {
        Timer?.Stop();
		GlobalEventService.OnMessage -= OnMessage;
    }
}
