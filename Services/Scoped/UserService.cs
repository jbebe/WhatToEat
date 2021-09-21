using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using WhatToEat.Helpers;
using WhatToEat.Services.Singleton;
using WhatToEat.Types;
using WhatToEat.Types.Enums;
using WhatToEat.Types.TableEntities;

namespace WhatToEat.Services.Scoped
{
  public class UserService
  {
    #region Service params

    public record LocalStorageUserData(string UserId);

    public UserData UserData { get; set; }

    public UserVote UserChoice { get;set; }

    public Dictionary<string, UserData> Users { get; set; } = new();

    public bool IsLoggedIn => UserData != null;

    private List<RestaurantData> Restaurants { get; set; }

    #endregion

    #region Injected services

    public ILocalStorageService LocalStorage { get; set; }

    public StorageService StorageService { get; set; }

    public EventService EventService { get; set; }

    public IHttpClientFactory HttpClient { get; }
    
    public AppConfiguration Config { get; }
    
    public IHttpContextAccessor HttpContextAccessor { get; }

    #endregion

    #region App events

    public event Func<Task> OnLocalChoiceChanged;

    public event Action OnLoginSuccessful;

    public event Func<ChoiceChanged, Task> OnBroadcastChoiceChanged;

    public event Func<PresenceChanged, Task> OnBroadcastPresenceChanged;

    public event Func<Task> OnBroadcastRestaurantChanged;

    #endregion

    public UserService(
      ILocalStorageService localStorage,
      StorageService storageService,
      EventService eventService,
      IHttpClientFactory httpClient,
      AppConfiguration config,
      IHttpContextAccessor httpContextAccessor)
    {
      LocalStorage = localStorage;
      StorageService = storageService;
      EventService = eventService;
      HttpClient = httpClient;
      Config = config;
      HttpContextAccessor = httpContextAccessor;
      EventService.OnMessage += msg =>
      {
        switch (msg.Type)
        {
          case BroadcastEventType.ChoiceChanged:
            OnBroadcastChoiceChanged?.Invoke((ChoiceChanged)msg);
            break;
          case BroadcastEventType.PresenceChanged:
            OnBroadcastPresenceChanged?.Invoke((PresenceChanged)msg);
            break;
          case BroadcastEventType.RestaurantChanged:
            OnBroadcastRestaurantChanged?.Invoke();
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      };
    }

    public async Task LoginAsync()
    {
      // Read UserId from local storage
      var localUserInfo = await LocalStorage.GetItemAsync<LocalStorageUserData>(Config.Constants.LocalStorageUserDataKey);
      if (localUserInfo == null)
      {
        localUserInfo = await CreateUserAsync();
      }
      else
      {
        UserData = await StorageService.GetUserAsync(localUserInfo.UserId);
        if (UserData == null)
          localUserInfo = await CreateUserAsync();
      };

      UserData ??= await StorageService.GetUserAsync(localUserInfo.UserId);

      await UpdateUserListAsync();

      OnLoginSuccessful?.Invoke();
    }

    private async Task<LocalStorageUserData> CreateUserAsync()
    {
      // Create user from Ad Auth endpoint user object & store Id in local storage
      var userInfo = await QueryUserDataAsync();
      await StorageService.CreateUserAsync(userInfo);
      var localUserInfo = new LocalStorageUserData(UserId: userInfo.GetUserId());
      await LocalStorage.SetItemAsync(Config.Constants.LocalStorageUserDataKey, localUserInfo);
      
      return localUserInfo;
    }

    public async Task UpdateUserListAsync()
    {
      Users = (await StorageService.GetUsersAsync()).ToDictionary(x => x.GetUserId());
    }

    private async Task<UserData> QueryUserDataAsync()
    {
      var client = HttpClient.CreateClient();
      AdUserData adUserData;
      client.BaseAddress = new Uri((HttpContextAccessor.HttpContext.Request.IsHttps ? "https://" : "http://") + HttpContextAccessor.HttpContext.Request.Host.Value);
      var response = await client.GetAsync(Config.Constants.AdUserInfoPath);
      if (response.StatusCode == HttpStatusCode.OK)
      {
        adUserData = (await response.Content.ReadFromJsonAsync<AdUserData[]>())!.First();
      }
      else
      {
        // Test user
        var randomNumber = new Random().Next(0, int.MaxValue);
        adUserData = JsonSerializer.Deserialize<AdUserData[]>(
          @"[{""user_claims"":[{""typ"":""./emailaddress"",""val"":""test+whattoeat+" +
          randomNumber +
          @"@tresorium.hu""},{""typ"":""name"",""val"":""Teszt " +
          randomNumber +
          @"""}]}]")!.First();
      }
      var userId = adUserData.UserClaims.Single(x => x.Typ.EndsWith("/emailaddress")).Val.ToLowerMd5Hash();
      var fullName = adUserData.UserClaims.Single(x => x.Typ == "name").Val;
      return new UserData(userId, fullName);
    }

    public async Task SetVoteAsync(List<string> ids)
    {
      if (!ids.Any())
        await StorageService.DeleteVoteAsync(UserData.GetUserId());
      else
        await StorageService.UpsertVoteAsync(UserData.GetUserId(), ids);

      OnLocalChoiceChanged?.Invoke();
      EventService.CreateMessage(new ChoiceChanged{ Type = BroadcastEventType.ChoiceChanged });
    }

    public async Task<List<string>> GetChoicesAsync()
    {
      UserChoice = await StorageService.GetVoteAsync(UserData.GetUserId());
      if (UserChoice == null)
        return new List<string>();

      var choices = UserChoice.GetChoicesTyped();
      return (await StorageService.GetRestaurantsAsync())
        .Where(x => choices.Contains(x.RowKey))
        .Select(x => x.Name).ToList();
    }

    public async Task<List<UserVote>> GetAllChoicesAsync(bool todayOnly)
    {
      var choices = await StorageService.GetVotesAsync(todayOnly ? DateTime.UtcNow : null);
      return choices;
    }

    public void SendPresence()
    {
      EventService.CreateMessage(new PresenceChanged(UserData.GetUserId()));
    }

    public void SendRestaurantUpdate()
    {
      EventService.CreateMessage(new RestaurantChanged());
    }

    public async Task<List<RestaurantData>> GetRestaurantsAsync(bool forceReload = false)
    {
      if (Restaurants == null || forceReload)
      {
        Restaurants = await StorageService.GetRestaurantsAsync();
      }

      return Restaurants;
    }
  }
}
