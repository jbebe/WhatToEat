namespace WhatToEat.App.Storage.Model;

public class Restaurant
{
    public string Id { get; set; }

    public string Name { get; set; }

    public List<string> PaymentMethods { get; }
}