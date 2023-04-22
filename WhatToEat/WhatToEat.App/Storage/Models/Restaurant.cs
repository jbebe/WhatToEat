using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using WhatToEat.App.Common;
using WhatToEat.App.Storage.Converters;

namespace WhatToEat.App.Storage.Model;

[PrimaryKey(nameof(Id))]
public class Restaurant : IEntityTypeConfiguration<Restaurant>
{
    public Id<Restaurant> Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public List<PaymentMethod> PaymentMethods { get; set; } = default!;

	public List<Vote> Votes { get; set; } = default!;

	public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.Property(p => p.Id).AddConverter();
		builder.Property(p => p.PaymentMethods).AddConverter();
    }
}