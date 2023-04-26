namespace WhatToEat.App.Common;

public static class RandomHelper
{
	public static T PickEnumValueByHash<T>(string hash) where T : Enum
	{
		var intValue = hash.Sum(x => (int)x);
		var enumValues = Enum.GetValues(typeof(T)).Cast<T>().ToArray();

		return enumValues[intValue % enumValues.Length];
	}
}
