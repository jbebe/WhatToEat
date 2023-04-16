using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatToEat.App.Storage.Model;

public class User : IEntityTypeConfiguration<User>
{
    public string Id { get; set; } = default!;

	public string Name { get; set; } = default!;

	public List<Vote> Votes { get; set; } = default!;

	[NotMapped]
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
	}
}
