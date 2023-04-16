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
builder.Services.Configure<WhatToEatSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddSingleton<StorageContext>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<RestaurantRepository>();
builder.Services.AddScoped<VoteRepository>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
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
