using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WhatToEat.App.Common;
using WhatToEat.App.Storage.Converters;
using System.ComponentModel.DataAnnotations;

namespace WhatToEat.App.Storage.Model;

[PrimaryKey(nameof(Id))]
public class User : IEntityTypeConfiguration<User>
{
    public Id<User> Id { get; set; } = default!;

	public string Name { get; set; } = default!;

	public List<Vote> Votes { get; set; } = default!;

	public bool Admin { get; set; }

	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.Property(x => x.Id).AddConverter();
		builder.HasMany(p => p.Votes)
			.WithOne(p => p.User)
			.HasForeignKey(p => p.UserId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
