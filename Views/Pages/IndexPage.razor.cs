using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Threading.Tasks;
using WhatToEat.Services.Scoped;

namespace WhatToEat.Views.Pages
{
  public partial class IndexPage : ComponentBase
  {
    [Inject]
    protected IDialogService DialogService { get; set; }

    [Inject]
    protected IJSRuntime JsRuntime { get; set; }

    [Inject]
    protected UserService UserService { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRun)
    {
      if (!UserService.IsLoggedIn)
      {
        await UserService.LoginAsync();
        StateHasChanged();
      }
    }
  }
}
