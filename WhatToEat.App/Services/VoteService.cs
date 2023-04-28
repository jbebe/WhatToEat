using Microsoft.EntityFrameworkCore;
using WhatToEat.App.Common;
using WhatToEat.App.Services.Models;
using WhatToEat.App.Storage.Model;
using WhatToEat.App.Storage.Repositories;

namespace WhatToEat.App.Services;

public sealed class VoteService : AsyncServiceBase
{
	private VoteRepository VoteRepository { get; set; }

	private GlobalEventService GlobalEventService { get; set; }

	public IReadOnlyList<Vote> Votes { get; set; } = new List<Vote>();

	public Vote? GetUserVote(Id<User> userId) => Votes.FirstOrDefault(x => x.UserId == userId.Value);

	public event Func<Task>? OnChanged;

	public VoteService(
	  VoteRepository voteRepository,
	  GlobalEventService globalEventService)
	{
		VoteRepository = voteRepository;
		GlobalEventService = globalEventService;
		GlobalEventService.OnMessage -= OnMessageAsync;
		GlobalEventService.OnMessage += OnMessageAsync;

		// Initial vote list
		UpdateVotesAsync().Wait();
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

    public async Task CastVoteAsync(Id<User> userId, List<Restaurant> restaurants, CancellationToken cancellationToken)
	{
		await VoteRepository.CreateOrUpdateAsync(userId, restaurants, cancellationToken);
		GlobalEventService.Send<VoteChanged>();
	}

	public override void Dispose()
	{
		GlobalEventService.OnMessage -= OnMessageAsync;
	}
}