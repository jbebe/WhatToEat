using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using Blazored.LocalStorage;
using WhatToEat.Services;
using WhatToEat.Helpers;
using Microsoft.AspNetCore.Http;

namespace WhatToEat
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      DevelopmentHelper.SetupTestEnvironmentIfNeeded(Configuration);
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddHttpClient();
      services.AddRazorPages();
      services.AddServerSideBlazor();
      services.AddMudServices();
      services.AddBlazoredLocalStorage();
      services.AddSingleton<EventService>();
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddScoped<StorageService>();
      services.AddScoped<UserService>();
      services.Configure<RazorPagesOptions>(options => options.RootDirectory = "/Views/Pages");
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
