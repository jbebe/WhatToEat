using System;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Threading.Tasks;
using WhatToEat.Services;

namespace WhatToEat.Views.Pages
{
  public partial class IndexPage : ComponentBase
  {
    public bool IsLoggedIn { get; set; }

    [Inject]
    protected IDialogService DialogService { get; set; }

    [Inject]
    protected IJSRuntime JsRuntime { get; set; }

    [Inject]
    protected UserService UserService { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRun)
    {
      if (!IsLoggedIn)
      {
        await UserService.LoginAsync();
        IsLoggedIn = true;
        StateHasChanged();
      }
    }
  }
}
