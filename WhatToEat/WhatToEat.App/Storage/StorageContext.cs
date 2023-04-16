using Microsoft.EntityFrameworkCore;
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

	public DbSet<T> GetDbSet<T>() where T : class => 
        (typeof(T).Name switch
	    {
		    nameof(User) => Users as DbSet<T>,
		    nameof(Restaurant) => Restaurants as DbSet<T>,
		    nameof(Vote) => Votes as DbSet<T>,
		    _ => throw new ArgumentOutOfRangeException(nameof(T))
	    })!;
}
