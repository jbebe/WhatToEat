using WhatToEat.App.Services.Models;

namespace WhatToEat.App.Services;

public class BroadcastService
{
	public event Func<BroadcastMessage, Task>? OnMessageAsync;

	public void SendMessage(BroadcastMessage message) => OnMessageAsync?.Invoke(message);

	public void SendMessage<T>() where T: BroadcastMessage, new() => SendMessage(new T());
}
