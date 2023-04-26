using System.Security.Cryptography;
using System.Text;
using WhatToEat.App.Storage.Dtos;

namespace WhatToEat.App.Common;

public static class ModelHelpers
{
    public static string GenerateId() => Guid.NewGuid().ToString("D");

    public static string GetPasswordHash(string password) => 
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
}
