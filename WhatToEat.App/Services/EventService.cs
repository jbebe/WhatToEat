using WhatToEat.App.Services.Models;

namespace WhatToEat.App.Services;

public abstract class EventService
{
	public event Func<BroadcastMessage, Task>? OnMessage;

	public void Send(BroadcastMessage message) => OnMessage?.Invoke(message);

	public void Send<T>() where T : BroadcastMessage, new() => Send(new T());
}

public class GlobalEventService: EventService { }

public class LocalEventService : EventService 
{
    public LocalEventService()
    {
        Console.WriteLine($"{nameof(LocalEventService)} initialized");
    }
}
