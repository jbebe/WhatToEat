namespace WhatToEat.App.Storage.Model;

public class Vote
{
    public long Date { get; set; }

    public User User { get; set; }

    public ICollection<Restaurant> Restaurants { get; set; }
}
