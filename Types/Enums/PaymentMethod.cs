using System.Collections.Generic;

namespace WhatToEat.Types.Enums
{
  public enum PaymentMethod
  {
    Cash = 1 << 0,
    BankCard = 1 << 1,
    SzepCard = 1 << 2,
  }

  public static class PaymentMethodExtensions
  {
    public static List<PaymentMethod> ToValues(this PaymentMethod method)
    {
      var output = new List<PaymentMethod>();
      for (var i = 0; i < sizeof(PaymentMethod); ++i)
      {
        var enumValue = (PaymentMethod)(1 << i);
        var isSet = (method & enumValue) != 0;
        if (isSet)
          output.Add(enumValue);
      }

      return output;
    }
  }
}
