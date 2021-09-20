using System;
using System.Collections.Generic;
using System.Linq;

namespace WhatToEat.Helpers
{
  public class UiHelper
  {
    public static Func<string, IEnumerable<string>> ValidateText(int min = 0, int max = int.MaxValue)
    {
      IEnumerable<string> validate(string input)
      {
        if (input?.Length < min) yield return "Password is too short!";
        if (input?.Length > max) yield return "Password is too long!";
      }

      return validate;
    }

    public static T PickEnumValueByHash<T>(string hash) where T: Enum
    {
      var intValue = hash.Sum(x => (int)x);
      var rnd = new Random(intValue);
      var enumValues = Enum.GetValues(typeof(T)).Cast<T>().ToList();
      var rndValue = rnd.Next(enumValues.Count);
      var enumValue = enumValues[rndValue];
      return enumValue;
    }
  }
}
