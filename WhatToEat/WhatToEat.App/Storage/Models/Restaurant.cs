using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using WhatToEat.App.Common;

namespace WhatToEat.App.Storage.Model;

public class Restaurant : IEntityTypeConfiguration<Restaurant>
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public List<PaymentMethod> PaymentMethods { get; set; } = default!;

    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.HasKey(x => x.Id);
        JsonSerializerOptions opts = null!;
        builder.Property(p => p.PaymentMethods).HasConversion(
            x => JsonSerializer.Serialize(x, opts),
            x => JsonSerializer.Deserialize<List<PaymentMethod>>(x, opts)!,
            new ValueComparer<List<PaymentMethod>>(
                (a, b) => !a!.Except(b!).Any() && !b!.Except(a!).Any(),
                x => x.GetHashCode()
            )
        );
    }
}