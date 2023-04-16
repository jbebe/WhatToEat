using MudBlazor.Services;
using WhatToEat.App.Server;
using WhatToEat.App.Storage;
using WhatToEat.App.Storage.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.Configure<WhatToEatSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddSingleton<StorageContext>();
builder.Services.AddScoped<UserRepository>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.InitDummyDatabase();
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
