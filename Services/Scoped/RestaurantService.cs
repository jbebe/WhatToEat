using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhatToEat.Services.Singleton;
using WhatToEat.Types;
using WhatToEat.Types.Enums;
using WhatToEat.Types.TableEntities;

namespace WhatToEat.Services.Scoped
{
  public sealed class RestaurantService: IDisposable
  {
    private List<RestaurantData> Restaurants { get; set; }

    public EventService EventService { get; set; }

    public StorageService StorageService { get; set; }

    public event Func<Task> OnBroadcastRestaurantChanged;

    public RestaurantService(
      EventService eventService,
      StorageService storageService)
    {
      StorageService = storageService;
      EventService = eventService;
      EventService.OnMessage += OnMessage;
    }

    private void OnMessage(BroadcastMessage message)
    {
      if (message.Type == BroadcastEventType.RestaurantChanged)
        OnBroadcastRestaurantChanged?.Invoke();
    }

    public void SendRestaurantUpdate()
    {
      EventService.CreateMessage(new RestaurantChanged());
    }

    public async Task<List<RestaurantData>> GetRestaurantsAsync(bool forceReload = false)
    {
      if (Restaurants == null || forceReload)
        Restaurants = await StorageService.GetRestaurantsAsync();

      return Restaurants;
    }

    public void Dispose() => EventService.OnMessage -= OnMessage;
  }
}
