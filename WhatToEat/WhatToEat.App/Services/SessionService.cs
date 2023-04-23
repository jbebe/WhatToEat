using Blazored.LocalStorage;
using WhatToEat.App.Common;
using WhatToEat.App.Services.Models;
using WhatToEat.App.Storage.Model;
using WhatToEat.App.Storage.Repositories;
using static MudBlazor.CategoryTypes;

namespace WhatToEat.App.Services;

public class SessionService
{
	record Credentials(string Email, string PasswordHash);

	public User? User { get; set; } = default!;
	
	private UserRepository UserRepository { get; }

	private LocalEventService LocalEventService { get; }

    ILocalStorageService LocalStorage { get; }

    public bool IsLoggedIn => User != null;

	public SessionService(
		UserRepository userRepository,
		LocalEventService localEventService,
        ILocalStorageService localStorage
    )
	{
		UserRepository = userRepository;
		LocalEventService = localEventService;
		LocalStorage = localStorage;
    }

    public async Task<bool> TryAutoLoginAsync(CancellationToken cancellationToken)
    {
		var credentials = await LocalStorage.GetItemAsync<Credentials>("access");
		if (!new[] { credentials?.Email, credentials?.PasswordHash }.Any(string.IsNullOrWhiteSpace))
		{
            User = await UserRepository.GetAsync(credentials!.Email, credentials.PasswordHash, cancellationToken);
            LocalEventService.Send<LoggedIn>();
        }
        
        return User != null;
    }

    public async Task<bool> LoginAsync(LoginForm form, CancellationToken cancellationToken)
	{
		var hash = ModelHelpers.GetPasswordHash(form.Password);
        await LocalStorage.SetItemAsync("access", new Credentials(form.Email, hash));
        User = await UserRepository.GetAsync(form.Email, hash, cancellationToken);
		LocalEventService.Send<LoggedIn>();

		return User != null;
	}
}
