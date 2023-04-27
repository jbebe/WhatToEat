using Microsoft.EntityFrameworkCore;
using WhatToEat.App.Services.Models;
using WhatToEat.App.Storage.Model;
using WhatToEat.App.Storage.Repositories;

namespace WhatToEat.App.Services;

public sealed class VoteService : AsyncServiceBase
{
	private SessionService SessionService { get; set; }

	private VoteRepository VoteRepository { get; set; }

	private GlobalEventService GlobalEventService { get; set; }

	private LocalEventService LocalEventService { get; set; }

	public IReadOnlyList<Vote> Votes { get; set; } = new List<Vote>();

	public Vote? Vote => Votes.FirstOrDefault(x => x.UserId == SessionService.User?.Id);

	public event Func<Task>? OnChanged;

	public VoteService(
	  SessionService sessionService,
	  VoteRepository voteRepository,
	  GlobalEventService globalEventService,
	  LocalEventService localEventService)
	{
		SessionService = sessionService;
		VoteRepository = voteRepository;
		GlobalEventService = globalEventService;
		LocalEventService = localEventService;
		GlobalEventService.OnMessage -= OnMessageAsync;
		GlobalEventService.OnMessage += OnMessageAsync;
		LocalEventService.OnMessage -= OnMessageAsync;
		LocalEventService.OnMessage += OnMessageAsync;
#pragma warning disable CS4014
		UpdateVotesAsync();
#pragma warning restore CS4014
	}

	private async Task OnMessageAsync(BroadcastMessage message)
	{
		if (message.Type == BroadcastEventType.VoteChanged || message.Type == BroadcastEventType.LoggedIn)
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
		GlobalEventService.Send<VoteChanged>();
	}

	public override void Dispose()
	{
		GlobalEventService.OnMessage -= OnMessageAsync;
		LocalEventService.OnMessage -= OnMessageAsync;
	}
}