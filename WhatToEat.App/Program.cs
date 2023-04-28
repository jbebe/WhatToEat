using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Server.Circuits;
using MudBlazor.Services;
using WhatToEat.App.Server;
using WhatToEat.App.Services;
using WhatToEat.App.Storage;
using WhatToEat.App.Storage.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(opts =>
{
    opts.RootDirectory = "/Razor/Internal";
});
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

// Load configurations
builder.Services.Configure<WhatToEatSettings>(builder.Configuration.GetSection("AppSettings"));

// Singletons
builder.Services.AddSingleton<StorageContext>();
builder.Services.AddSingleton<GlobalEventService>();
builder.Services.AddSingleton(serviceProvider => 
    serviceProvider.GetRequiredService<IConfiguration>().Get<WhatToEatSettings>()!);
// Storage services
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<RestaurantRepository>();
builder.Services.AddSingleton<VoteRepository>();
// Business logic services
builder.Services.AddSingleton<RestaurantService>();
builder.Services.AddSingleton<VoteService>();
builder.Services.AddSingleton<PresenceService>();

// Internal services
builder.Services.AddScoped<CancellationService>();
builder.Services.Add(new ServiceDescriptor(
    typeof(CancellationToken), 
    (services) => services.GetRequiredService<CancellationService>()!.CancellationToken, ServiceLifetime.Scoped));
// Business logic services
builder.Services.AddScoped<LocalEventService>();
builder.Services.AddScoped<SessionService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    // Init test data
    await app.InitDummyDatabaseAsync();
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/Html");
app.Run();
