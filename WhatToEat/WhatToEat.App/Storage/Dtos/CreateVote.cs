using WhatToEat.App.Storage.Model;

namespace WhatToEat.App.Storage.Dtos;

public record CreateVote(User User, List<Restaurant> Restaurants);
