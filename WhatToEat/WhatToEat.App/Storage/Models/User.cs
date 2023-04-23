using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WhatToEat.App.Storage.Models;

namespace WhatToEat.App.Storage.Model;

[PrimaryKey(nameof(Id))]
public class User : SingleKeyedEntityBase<User>, IEntityTypeConfiguration<User>
{
    public override string Id { get; set; } = default!;

	public string Name { get; set; } = default!;

	public string Email { get; set; } = default!;

	public string PasswordHash { get; set; } = default!;

	public List<Vote> Votes { get; set; } = default!;

	public bool Admin { get; set; }

	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasMany(p => p.Votes)
			.WithOne(p => p.User)
			.HasForeignKey(p => p.UserId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
