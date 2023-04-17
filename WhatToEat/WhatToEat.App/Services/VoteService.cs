using WhatToEat.App.Common;
using WhatToEat.App.Services.Models;
using WhatToEat.App.Storage.Model;
using WhatToEat.App.Storage.Repositories;

namespace WhatToEat.App.Services;

public sealed class VoteService : IDisposable
{
	private Vote? Vote { get; set; }

	private SessionService SessionService { get; set; }

	private VoteRepository VoteRepository { get; set; }

	private BroadcastService BroadcastService { get; set; }

	public event Func<VoteChanged, Task>? OnBroadcastVoteChanged;

	public VoteService(
	  SessionService sessionService,
	  VoteRepository voteRepository,
	  BroadcastService broadcastService)
	{
		SessionService = sessionService;
		VoteRepository = voteRepository;
		BroadcastService = broadcastService;
		BroadcastService.OnMessage += OnMessage;
	}

	private void OnMessage(BroadcastMessage message)
	{
		if (message.Type == BroadcastEventType.VoteChanged)
		{
			OnBroadcastVoteChanged?.Invoke((VoteChanged)message);
		}
	}

	public async Task CastVoteAsync(List<Id<Restaurant>> ids, CancellationToken cancellationToken)
	{
		await VoteRepository.CreateOrUpdateAsync(SessionService.User, ids, cancellationToken);

		BroadcastService.SendMessage(new VoteChanged());
	}

	public async Task<List<Id<Restaurant>>> GetVoteAsync(CancellationToken cancellationToken)
	{
		Vote = await VoteRepository.GetAsync(SessionService.User, cancellationToken);
		if (Vote == null)
			return new List<Id<Restaurant>>();

		return Vote.RestaurantIds;
	}

	public async Task<List<Vote>> GetVotesAsync(CancellationToken cancellationToken)
	{
		var votes = await VoteRepository.GetAllAsync(cancellationToken);
		return votes;
	}

	public void Dispose()
	{
		BroadcastService.OnMessage -= OnMessage;
	}
}