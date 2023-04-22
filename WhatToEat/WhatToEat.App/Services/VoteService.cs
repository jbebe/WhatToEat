using Microsoft.EntityFrameworkCore;
using WhatToEat.App.Services.Models;
using WhatToEat.App.Storage.Model;
using WhatToEat.App.Storage.Repositories;

namespace WhatToEat.App.Services;

public sealed class VoteService : AsyncServiceBase
{
	private SessionService SessionService { get; set; }

	private VoteRepository VoteRepository { get; set; }

	private BroadcastService BroadcastService { get; set; }

	public IReadOnlyList<Vote> Votes { get; set; } = new List<Vote>();

	public Vote? Vote => Votes.FirstOrDefault(x => x.UserId == SessionService.User?.Id);

	public event Func<Task>? OnChanged;

	public VoteService(
	  SessionService sessionService,
	  VoteRepository voteRepository,
	  BroadcastService broadcastService)
	{
		SessionService = sessionService;
		VoteRepository = voteRepository;
		BroadcastService = broadcastService;
		BroadcastService.OnMessageAsync += OnMessageAsync;
#pragma warning disable CS4014
		UpdateVotesAsync();
#pragma warning restore CS4014
	}

	private async Task OnMessageAsync(BroadcastMessage message)
	{
		if (message.Type == BroadcastEventType.VoteChanged)
		{
			await UpdateVotesAsync();
			OnChanged?.Invoke();
		}
	}

	private async Task UpdateVotesAsync()
	{
		Votes = await VoteRepository.GetAllAsync(AddIncludes, CancellationToken.None);
	}

    private IQueryable<Vote> AddIncludes(DbSet<Vote> dbSet)
    {
		return dbSet
			.Include(x => x.Restaurants)
			.Include(x => x.User);
    }

    public async Task CastVoteAsync(List<Restaurant> restaurants, CancellationToken cancellationToken)
	{
		await VoteRepository.CreateOrUpdateAsync(SessionService.User!, restaurants, cancellationToken);
		BroadcastService.SendMessage<VoteChanged>();
	}

	public override void Dispose()
	{
		BroadcastService.OnMessageAsync -= OnMessageAsync;
	}
}