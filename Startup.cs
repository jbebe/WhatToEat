using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using Blazored.LocalStorage;
using WhatToEat.Helpers;
using Microsoft.AspNetCore.Http;
using WhatToEat.Services.Singleton;
using WhatToEat.Services.Scoped;
using WhatToEat.Types;

namespace WhatToEat
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = new AppConfiguration(configuration);
      DevelopmentHelper.SetupTestEnvironmentIfNeeded(Configuration);
    }

    public AppConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton<AppConfiguration>();
      services.AddHttpClient();
      services.AddRazorPages();
      services.AddServerSideBlazor();
      services.AddMudServices();
      services.AddBlazoredLocalStorage();
      services.AddSingleton<EventService>();
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddScoped<LoggerService>();
      services.AddScoped<StorageService>();
      services.AddScoped<UserService>();
      services.Configure<RazorPagesOptions>(options => options.RootDirectory = "/Views/Pages");
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
      }

      app.UseStaticFiles();

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapBlazorHub();
        endpoints.MapFallbackToPage("/_Host");
      });
    }
  }
}
