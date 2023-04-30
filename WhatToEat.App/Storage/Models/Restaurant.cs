using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhatToEat.App.Common;
using WhatToEat.App.Storage.Converters;
using WhatToEat.App.Storage.Models;

namespace WhatToEat.App.Storage.Model;

[PrimaryKey(nameof(Id))]
public class Restaurant : SingleKeyedEntityBase<Restaurant>, IEntityTypeConfiguration<Restaurant>
{
    public override string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public List<PaymentMethod> PaymentMethods { get; set; } = default!;

    public List<ConsumptionType> ConsumptionTypes { get; set; } = default!;

	public List<Vote> Votes { get; set; } = default!;

    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
		builder.Property(p => p.PaymentMethods).AddConverter();
		builder.Property(p => p.ConsumptionTypes).AddConverter();
    }
}
