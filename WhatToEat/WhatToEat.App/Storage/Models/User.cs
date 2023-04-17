using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WhatToEat.App.Common;
using WhatToEat.App.Storage.Converters;

namespace WhatToEat.App.Storage.Model;

public class User : IEntityTypeConfiguration<User>
{
    public Id<User> Id { get; set; } = default!;

	public string Name { get; set; } = default!;

	public List<Vote> Votes { get; set; } = default!;

	public bool Admin { get; set; }

	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasKey(x => x.Id);
		builder
			.HasMany(x => x.Votes)
			.WithOne(x => x.User)
			.HasForeignKey(x => x.UserId)
			.IsRequired()
			.OnDelete(DeleteBehavior.Cascade);
		builder.Property(x => x.Id).AddConverter();
	}
}
