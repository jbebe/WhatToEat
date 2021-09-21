using System;
using WhatToEat.Types;

namespace WhatToEat.Services.Singleton
{
  public class EventService
  {
    public event Action<BroadcastMessage> OnMessage;

    public void CreateMessage(BroadcastMessage message) => OnMessage?.Invoke(message);
  }
}
