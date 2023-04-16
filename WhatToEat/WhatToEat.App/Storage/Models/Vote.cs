using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using WhatToEat.App.Common;

namespace WhatToEat.App.Storage.Model;

public class Vote : IEntityTypeConfiguration<Vote>
{
    public DateTime Date { get; set; }

    public string UserId { get; set; } = default!;

    public User User { get; set; } = default!;

    public List<string> RestaurantIds { get; set; } = default!;

    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasKey(x => new { x.Date, x.UserId });
        builder.Property(p => p.Date).HasConversion(
            x => x.Date.Ticks,
            x => new DateTime(x),
            new ValueComparer<DateTime>(
                (a, b) => a.Ticks == b.Ticks,
                x => x.GetHashCode()
            )
        );
		JsonSerializerOptions opts = null!;
		builder.Property(p => p.RestaurantIds).HasConversion(
			x => JsonSerializer.Serialize(x, opts),
			x => JsonSerializer.Deserialize<List<string>>(x, opts)!,
			new ValueComparer<List<string>>(
				(a, b) => !a!.Except(b!).Any() && !b!.Except(a!).Any(),
				x => x.GetHashCode()
			)
		);
	}
}
