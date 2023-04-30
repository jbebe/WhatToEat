using MudBlazor;

namespace WhatToEat.App.Services.Models;

public class RestaurantForm
{
    public string Name { get; set; }

    public bool CashPayment { get; set; }

    public bool BankCardPayment { get; set; }

    public bool SzepCardPayment { get; set; }

    public bool DineIn { get; set; }

    public bool Takeaway { get; set; }

    public bool Delivery { get; set; }
}
