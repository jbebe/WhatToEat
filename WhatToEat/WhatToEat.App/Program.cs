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

// Load configurations
builder.Services.Configure<WhatToEatSettings>(builder.Configuration.GetSection("AppSettings"));

// Singletons
builder.Services.AddSingleton<StorageContext>();
builder.Services.AddSingleton<BroadcastService>();

// Storage services
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<RestaurantRepository>();
builder.Services.AddScoped<VoteRepository>();

// Business logic services
builder.Services.AddScoped<VoteService>();
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
