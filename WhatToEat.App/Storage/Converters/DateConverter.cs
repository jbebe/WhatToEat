using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WhatToEat.App.Storage.Converters;

public static class DateConverter
{
	public static void AddConverter(this PropertyBuilder<DateTime> builder)
	{
		builder.HasConversion(
			new ValueConverter<DateTime, long>(
				x => x.Date.Ticks,
				x => new DateTime(x)
			),
			new ValueComparer<DateTime>(
				(a, b) => a.Ticks == b.Ticks,
				x => x.GetHashCode()
			)
		);
	}

}
