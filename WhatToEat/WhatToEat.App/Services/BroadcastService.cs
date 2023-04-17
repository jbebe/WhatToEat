using WhatToEat.App.Services.Models;

namespace WhatToEat.App.Services;

public class BroadcastService
{
	public event Action<BroadcastMessage>? OnMessage;

	public void SendMessage(BroadcastMessage message) => OnMessage?.Invoke(message);
}
