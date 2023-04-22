using System.Collections.Immutable;
using WhatToEat.App.Common;
using WhatToEat.App.Services.Models;
using WhatToEat.App.Storage.Model;
using WhatToEat.App.Storage.Repositories;

namespace WhatToEat.App.Services;

public sealed class RestaurantService : AsyncServiceBase
{
	private RestaurantRepository RestaurantRepository { get; set; }

	public BroadcastService BroadcastService { get; set; }

	public IReadOnlyDictionary<Id<Restaurant>, Restaurant> Restaurants { get; private set; } = 
		new Dictionary<Id<Restaurant>, Restaurant>();

	public event Func<Task>? OnChanged;

	public RestaurantService(
	  BroadcastService eventService,
	  RestaurantRepository restaurantRepository, 
	  CancellationTokenSource cancellationTokenSource
	) : base(cancellationTokenSource)
	{
		BroadcastService = eventService;
		RestaurantRepository = restaurantRepository;
		BroadcastService.OnMessageAsync -= OnMessageAsync;
		BroadcastService.OnMessageAsync += OnMessageAsync;
	}

	private async Task OnMessageAsync(BroadcastMessage message)
	{
		if (message.Type == BroadcastEventType.RestaurantChanged)
		{
			await UpdateRestaurantsAsync();
			OnChanged?.Invoke();
		}
	}

	public void SendRestaurantUpdate()
	{
		BroadcastService.SendMessage<RestaurantChanged>();
	}

	private async Task UpdateRestaurantsAsync()
	{
		var restaurantList = await RestaurantRepository.GetAllAsync(CancellationToken);
		Restaurants = restaurantList.ToImmutableDictionary(x => x.Id);
	}

	public override void Dispose()
	{
		BroadcastService.OnMessageAsync -= OnMessageAsync;
	}
}
