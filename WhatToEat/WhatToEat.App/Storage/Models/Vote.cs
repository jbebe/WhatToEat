using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WhatToEat.App.Storage.Model;

public class Vote : IEntityTypeConfiguration<Vote>
{
    public DateTime Date { get; set; }

    public string UserId { get; set; }

    public User User { get; set; }

    public List<Restaurant> Restaurants { get; set; }

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
    }
}
