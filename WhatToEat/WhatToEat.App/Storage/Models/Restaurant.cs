using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using WhatToEat.App.Common;

namespace WhatToEat.App.Storage.Model;

public class Restaurant : IEntityTypeConfiguration<Restaurant>
{
    public string Id { get; set; }

    public string Name { get; set; }

    public List<PaymentMethod> PaymentMethods { get; set; }

    public List<Vote> Votes { get; set; }

    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Votes).WithMany(x => x.Restaurants);

        JsonSerializerOptions opts = null!;
        builder.Property(p => p.PaymentMethods).HasConversion(
            x => JsonSerializer.Serialize(x, opts),
            x => JsonSerializer.Deserialize<List<PaymentMethod>>(x, opts)!,
            new ValueComparer<List<PaymentMethod>>(
                (a, b) => !a.Except(b).Any() && !b.Except(a).Any(),
                x => x.GetHashCode()
            )
        );
    }
}