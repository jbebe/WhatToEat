using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhatToEat.App.Common;
using WhatToEat.App.Storage.Converters;

namespace WhatToEat.App.Storage.Model;

public class Restaurant : IEntityTypeConfiguration<Restaurant>
{
    public Id<Restaurant> Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public List<PaymentMethod> PaymentMethods { get; set; } = default!;

    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(p => p.Id).AddConverter();
		builder.Property(p => p.PaymentMethods).AddConverter();
    }
}