using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhatToEat.App.Storage.Converters;

namespace WhatToEat.App.Storage.Model;

[PrimaryKey(nameof(Date), nameof(UserId))]
public class Vote : IEntityTypeConfiguration<Vote>
{
	public DateTime Date { get; set; }

	public string UserId { get; set; } = default!;

    public User User { get; set; } = default!;

    public List<Restaurant> Restaurants { get; set; } = default!;

    public void Configure(EntityTypeBuilder<Vote> builder)
    {
		builder.Property(p => p.Date).AddConverter();
		builder.HasOne(p => p.User)
			.WithMany(p => p.Votes)
			.HasForeignKey(p => p.UserId)
			.IsRequired()
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasMany(p => p.Restaurants)
			.WithMany(p => p.Votes);
	}
}
