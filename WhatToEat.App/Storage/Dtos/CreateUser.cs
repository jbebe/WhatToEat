namespace WhatToEat.App.Storage.Dtos;

public record CreateUser(string Name, string Email, string Password, bool Admin = false);
