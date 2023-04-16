namespace WhatToEat.App.Common;

public static class ModelHelpers
{
    public static string GenerateId() => Guid.NewGuid().ToString();
}
