using System;

namespace WhatToEat.Services
{
  public enum BroadcastEventType
  {
    ChoiceChanged,
    PresenceChanged,
    RestaurantChanged,
  }

  public class BroadcastMessage
  {
    public BroadcastEventType Type { get; set; }

    protected BroadcastMessage(BroadcastEventType presenceChanged)
    {
      Type = presenceChanged;
    }
  }

  public class ChoiceChanged: BroadcastMessage
  {
    public ChoiceChanged(): base(BroadcastEventType.ChoiceChanged)
    {
    }
  }

  public class PresenceChanged: BroadcastMessage
  {
    public string UserId { get; }

    public PresenceChanged(string userId): base(BroadcastEventType.PresenceChanged)
    {
      UserId = userId;
    }
  }

  public class RestaurantChanged : BroadcastMessage
  {
    public RestaurantChanged() : base(BroadcastEventType.RestaurantChanged)
    {
    }
  }


  public class EventService
  {
    public event Action<BroadcastMessage> OnMessage;

    public void CreateMessage(BroadcastMessage message) => OnMessage?.Invoke(message);
  }
}
