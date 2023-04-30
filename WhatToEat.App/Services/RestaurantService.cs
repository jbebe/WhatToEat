﻿using System.Collections.Immutable;
using WhatToEat.App.Common;
using WhatToEat.App.Services.Models;
using WhatToEat.App.Storage.Model;
using WhatToEat.App.Storage.Repositories;

namespace WhatToEat.App.Services;

public sealed class RestaurantService: AsyncServiceBase
{
	private RestaurantRepository RestaurantRepository { get; set; }

	private GlobalEventService GlobalEventService { get; set; }

	public IReadOnlyDictionary<Id<Restaurant>, Restaurant> Restaurants { get; private set; } = 
		new Dictionary<Id<Restaurant>, Restaurant>();

	public event Func<Task>? OnChanged;

	public RestaurantService(
	  GlobalEventService globalEventService,
	  RestaurantRepository restaurantRepository)
	{
        GlobalEventService = globalEventService;
		RestaurantRepository = restaurantRepository;
		GlobalEventService.OnMessage -= OnMessageAsync;
		GlobalEventService.OnMessage += OnMessageAsync;

		// Initial restaurant list
		UpdateRestaurantsAsync().Wait();
	}

	private async Task OnMessageAsync(BroadcastMessage message)
	{
		if (message.Type == BroadcastEventType.RestaurantChanged)
		{
			await UpdateRestaurantsAsync();
			OnChanged?.Invoke();
		}
	}

	private async Task UpdateRestaurantsAsync()
	{
		var restaurantList = await RestaurantRepository.GetAllAsync(null, CancellationToken.None);
		Restaurants = restaurantList.ToImmutableDictionary(x => x.IdTyped);
	}

	public async Task CreateRestaurantAsync(RestaurantForm form)
	{
		var paymentMethods = new List<PaymentMethod>();
		if (form.CashPayment) paymentMethods.Add(PaymentMethod.Cash);
		if (form.BankCardPayment) paymentMethods.Add(PaymentMethod.BankCard);
		if (form.SzepCardPayment) paymentMethods.Add(PaymentMethod.SzepCard);
		var consumptionTypes = new List<ConsumptionType>();
		if (form.DineIn) consumptionTypes.Add(ConsumptionType.DineIn);
		if (form.Takeaway) consumptionTypes.Add(ConsumptionType.Takeaway);
		if (form.Delivery) consumptionTypes.Add(ConsumptionType.Delivery);
		await RestaurantRepository.CreateAsync(form.Name, paymentMethods, consumptionTypes);
		GlobalEventService.Send<RestaurantChanged>();
	}

	public override void Dispose()
	{
		GlobalEventService.OnMessage -= OnMessageAsync;
	}
}
