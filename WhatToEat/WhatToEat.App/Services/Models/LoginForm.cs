using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WhatToEat.App.Services.Models;

public class LoginForm
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [StringLength(30, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 4)]
    public string Password { get; set; } = "";

    public LoginForm(string email, string password)
    {
        Email = email;
		Password = password;
	}
}
