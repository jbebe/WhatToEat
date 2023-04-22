using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WhatToEat.App.Common;

namespace WhatToEat.App.Storage.Converters;

public static class IdConverter
{
	public static void AddConverter<T>(this PropertyBuilder<Id<T>> builder)
	{
		builder.HasConversion(
			new ValueConverter<Id<T>, string>(
				x => x.Value,
				x => new Id<T>(x)
			)
		);
	}
}
