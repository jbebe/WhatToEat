using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Http;
using WhatToEat.Helpers;
using WhatToEat.Types;
using WhatToEat.Types.TableEntities;

namespace WhatToEat.Services.Scoped
{
  public class UserService
  {
    #region Service params

    public record LocalStorageUserData(string UserId);

    public UserData UserData { get; set; }

    public Dictionary<string, UserData> Users { get; set; } = new();

    public bool IsLoggedIn => UserData != null;

    #endregion

    #region Injected services

    public ILocalStorageService LocalStorage { get; set; }

    public StorageService StorageService { get; set; }

    public IHttpClientFactory HttpClient { get; }
    
    public AppConfiguration Config { get; }
    
    public IHttpContextAccessor HttpContextAccessor { get; }

    #endregion

    #region App events

    public event Action OnLoginSuccessful;

    #endregion

    public UserService(
      ILocalStorageService localStorage,
      StorageService storageService,
      IHttpClientFactory httpClient,
      AppConfiguration config,
      IHttpContextAccessor httpContextAccessor)
    {
      LocalStorage = localStorage;
      StorageService = storageService;
      HttpClient = httpClient;
      Config = config;
      HttpContextAccessor = httpContextAccessor;
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
      // Create user from Ad Auth endpoint user object
      var userInfo = await QueryAdUserDataAsync();
      
      await StorageService.CreateUserAsync(userInfo);

      // Store id in local storage
      var localUserInfo = new LocalStorageUserData(UserId: userInfo.GetUserId());
      await LocalStorage.SetItemAsync(Config.Constants.LocalStorageUserDataKey, localUserInfo);
      
      return localUserInfo;
    }

    public async Task UpdateUserListAsync() =>
      Users = (await StorageService.GetUsersAsync()).ToDictionary(x => x.GetUserId());

    private async Task<UserData> QueryAdUserDataAsync()
    {
      if (Config.Constants.Environment == Types.Enums.AppEnvironment.Development)
        return DevelopmentHelper.CreateTestUserData();
      
      var client = HttpClient.CreateClient();
      client.BaseAddress = new Uri($"https://{HttpContextAccessor.HttpContext.Request.Host.Value}");
      var response = await client.GetAsync(Config.Constants.AdUserInfoPath);
      var adUserData = (await response.Content.ReadFromJsonAsync<AdUserData[]>())!.First();
      return new UserData(adUserData.GetUserId(), adUserData.GetFullName());
    }
  }
}
