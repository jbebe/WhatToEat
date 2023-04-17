namespace WhatToEat.App.Services.Models;

public enum BroadcastEventType
{
	VoteChanged,
	PresenceChanged,
	RestaurantChanged,
}

public record BroadcastMessage(BroadcastEventType Type);

public record VoteChanged() : BroadcastMessage(BroadcastEventType.VoteChanged);

public record PresenceChanged(string UserId) : BroadcastMessage(BroadcastEventType.PresenceChanged);

public record RestaurantChanged() : BroadcastMessage(BroadcastEventType.RestaurantChanged);