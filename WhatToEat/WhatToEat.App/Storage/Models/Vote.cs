using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using WhatToEat.App.Common;
using WhatToEat.App.Storage.Converters;

namespace WhatToEat.App.Storage.Model;

public class Vote : IEntityTypeConfiguration<Vote>
{
    public DateTime Date { get; set; }

    public Id<User> UserId { get; set; } = default!;

    public User User { get; set; } = default!;

    public List<Id<Restaurant>> RestaurantIds { get; set; } = default!;

    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasKey(x => new { x.Date, x.UserId });
        builder.Property(p => p.Date).AddConverter();
		builder.Property(p => p.UserId).AddConverter();
		builder.Property(p => p.RestaurantIds).AddConverter();
	}
}
