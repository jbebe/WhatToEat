using MudBlazor;

namespace WhatToEat.App.Services.Models;

public class RestaurantForm
{
    public string Name { get; set; }

    public bool CashPayment { get; set; }

    public bool BankCardPayment { get; set; }

    public bool SzepCardPayment { get; set; }
}
