using System;
using System.Threading.Tasks;
using WhatToEat.Services.Singleton;
using WhatToEat.Types;
using WhatToEat.Types.Enums;

namespace WhatToEat.Services.Scoped
{
  public sealed class PresenceService: IDisposable
  {
    public EventService EventService { get; set; }

    public UserService UserService { get; set; }

    public event Func<PresenceChanged, Task> OnBroadcastPresenceChanged;

    public PresenceService(
      EventService eventService,
      UserService userService
      )
    {
      UserService = userService;
      EventService = eventService;
      EventService.OnMessage += OnMessage;
    }

    private void OnMessage(BroadcastMessage message)
    {
      if (message.Type == BroadcastEventType.PresenceChanged)
      {
        OnBroadcastPresenceChanged?.Invoke((PresenceChanged)message);
      }
    }

    public void SendPresence()
    {
      EventService.CreateMessage(new PresenceChanged(UserService.UserData.GetUserId()));
    }

    public void Dispose() => EventService.OnMessage -= OnMessage;
  }
}
