using WhatToEat.App.Common;
using WhatToEat.App.Storage.Model;

namespace WhatToEat.App.Services.Models;

public enum BroadcastEventType
{
	VoteChanged,
	PresenceChanged,
	RestaurantChanged,
	LoggedIn,
}

public record BroadcastMessage(BroadcastEventType Type);

public record VoteChanged() : BroadcastMessage(BroadcastEventType.VoteChanged);

public record PresenceChanged(Id<User> UserId) : BroadcastMessage(BroadcastEventType.PresenceChanged);

public record RestaurantChanged() : BroadcastMessage(BroadcastEventType.RestaurantChanged);

public record LoggedIn() : BroadcastMessage(BroadcastEventType.LoggedIn);