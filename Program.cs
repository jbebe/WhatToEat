using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WhatToEat
{
  public class Program
  {
    /// <remarks>
    /// Required environment variables:
    ///   - WHATTOEAT_STORAGE_CONNSTR
    ///   - ADUSER_INFO_URL
    /// </remarks>
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
  }
}
