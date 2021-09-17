using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WhatToEat.Helpers
{
  public static class HashHelper
  {
    public static string GetRandomLowercaseHash()
    {
      return DateTime.UtcNow.Ticks.ToString("D").ToLowerMd5Hash();
    }

    /// <summary>
    /// Both input and output will be lowercased
    /// </summary>
    public static string ToLowerMd5Hash(this string text)
    {
      var hash = MD5.Create().ComputeHash(
        Encoding.UTF8.GetBytes(
          text.ToLowerInvariant()));
      return string.Join("", hash.Select(x => x.ToString("x2")));
    }
  }
}
