using System;
using System.Text.Json;
using WhatToEat.Services.Scoped;
using WhatToEat.Types;
using WhatToEat.Types.Enums;
using WhatToEat.Types.TableEntities;

namespace WhatToEat.Helpers
{
  public class DevelopmentHelper
  {
    public static void SetupTestEnvironmentIfNeeded(AppConfiguration config)
    {
      if (config.Constants.Environment == AppEnvironment.Production)
        return;

      // Create restaurants
      var storage = new StorageService(config);
      storage.CreateRestaurantAsync(new RestaurantData(
        "266f921c630140478db0e7a2a0190ea6", "Test Restaurant 1", PaymentMethod.Cash)).Wait();
      storage.CreateRestaurantAsync(new RestaurantData(
        "ff74cf7a4ced4e918391b9ce52ea42b1", "Test Restaurant 2", PaymentMethod.Cash | PaymentMethod.BankCard)).Wait();
      storage.CreateRestaurantAsync(new RestaurantData(
        "0734135f92e54c92b60bbe0317eee06a", "Test Restaurant 3", PaymentMethod.BankCard | PaymentMethod.SzepCard)).Wait();
    }

    public static UserData CreateTestUserData()
    {
      var randomNumber = new Random().Next(0, int.MaxValue);
      return new UserData($"test+whattoeat+{randomNumber}@tresorium.hu".ToLowerMd5Hash(), $"Test {randomNumber}");
    }
  }
}
