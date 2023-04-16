using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using System.Data.Common;
using WhatToEat.App.Server;
using WhatToEat.App.Storage.Model;

namespace WhatToEat.App.Storage;

public class StorageContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Vote> Votes { get; set; }

    public DbSet<Restaurant> Restaurants { get; set; }
    
    public string ConnectionString { get; }

    public StorageContext(IConfiguration configuration) 
    {
        ConnectionString = configuration.Get<WhatToEatSettings>()!.SQLite.ConnectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder) 
    {
        builder.UseSqlite(ConnectionString);
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
    }
}

class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
    }
}

class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasKey(x => x.Date);
        builder.HasOne(x => x.User);
        builder.HasMany(x => x.Restaurants);
    }
}

class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.HasKey(x => x.Id);
    }
}