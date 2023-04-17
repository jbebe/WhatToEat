using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using System.Text.Json.Serialization;
using WhatToEat.App.Common;
using WhatToEat.App.Storage.Model;

namespace WhatToEat.App.Storage.Converters;

public static class ListConverter
{
	public static void AddConverter<T>(this PropertyBuilder<List<T>> builder)
	{
		var opts = new JsonSerializerOptions();
		opts.Converters.Add(new JsonStringEnumConverter());
		opts.Converters.Add(new JsonIdConverter<Restaurant>());
		opts.Converters.Add(new JsonIdConverter<User>());
		opts.Converters.Add(new JsonIdConverter<Vote>());

		builder.HasConversion(
			new ValueConverter<List<T>, string>(
				x => JsonSerializer.Serialize(x, opts),
				x => JsonSerializer.Deserialize<List<T>>(x, opts)!
			),
			new ValueComparer<List<T>>(
				(a, b) => !a!.Except(b!).Any() && !b!.Except(a!).Any(),
				x => JsonSerializer.Serialize(x, opts).GetHashCode()
			)
		);
	}

}
