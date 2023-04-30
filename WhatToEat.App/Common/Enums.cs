namespace WhatToEat.App.Common;

public enum PaymentMethod
{
	Cash,
	BankCard,
	SzepCard,
}

public enum ConsumptionType
{
	DineIn,
	Takeaway,
	Delivery,
}

public static class ConsumptionTypeExtensions
{
	public static string GetDisplayName(this ConsumptionType consumptionType) => consumptionType switch
	{
		ConsumptionType.DineIn => "Dine-in",
		ConsumptionType.Takeaway => "Takeaway",
		ConsumptionType.Delivery => "Delivery",
		_ => consumptionType.ToString("G")
	};
}
