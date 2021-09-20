using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace WhatToEat.Services.Scoped
{
  public class LoggerService
  {
    public IJSRuntime JSRuntime { get; set; }

    public LoggerService(IJSRuntime jsRuntime)
    {
      JSRuntime = jsRuntime;
    }

    public async Task ClientLogAsync(params object[] objs)
    {
      await JSRuntime.InvokeVoidAsync("console.log", objs);
    }
  }
}
