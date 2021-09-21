using WhatToEat.Types.Enums;

namespace WhatToEat.Types
{
  public class BroadcastMessage
  {
    public BroadcastEventType Type { get; set; }

    protected BroadcastMessage(BroadcastEventType presenceChanged)
    {
      Type = presenceChanged;
    }
  }

  public class VoteChanged : BroadcastMessage
  {
    public VoteChanged() : base(BroadcastEventType.VoteChanged)
    {
    }
  }

  public class PresenceChanged : BroadcastMessage
  {
    public string UserId { get; }

    public PresenceChanged(string userId) : base(BroadcastEventType.PresenceChanged)
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
}
