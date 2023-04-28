using Blazored.LocalStorage;
using WhatToEat.App.Common;
using WhatToEat.App.Services.Models;
using WhatToEat.App.Storage.Model;
using WhatToEat.App.Storage.Repositories;

namespace WhatToEat.App.Services;

public class SessionService
{
	private const string CredentialsKey = "access";

	record Credentials(string Email, string PasswordHash);

	public User? User { get; set; } = default!;
	
	public bool IsLoggedIn => User != null;
	
	UserRepository UserRepository { get; }

    ILocalStorageService LocalStorage { get; }

    LocalEventService LocalEventService { get; }

	GlobalEventService GlobalEventService { get; }

	public SessionService(
		UserRepository userRepository,
		LocalEventService localEventService,
		GlobalEventService globalEventService,
        ILocalStorageService localStorage
    ){
		UserRepository = userRepository;
		LocalEventService = localEventService;
		GlobalEventService = globalEventService;
		LocalStorage = localStorage;
    }

    public async Task<bool> TryAutoLoginAsync(CancellationToken cancellationToken)
    {
		var credentials = await LocalStorage.GetItemAsync<Credentials>(CredentialsKey, cancellationToken);
		if (!new[] { credentials?.Email, credentials?.PasswordHash }.Any(string.IsNullOrWhiteSpace))
		{
            User = await UserRepository.GetAsync(credentials!.Email, credentials.PasswordHash, cancellationToken);
            LocalEventService.Send<LoggedIn>();
			GlobalEventService.Send(new PresenceChanged(User.IdTyped));
		}
        
        return User != null;
    }

    public async Task<bool> LoginAsync(LoginForm form, CancellationToken cancellationToken)
	{
		var hash = ModelHelpers.GetPasswordHash(form.Password);
        await LocalStorage.SetItemAsync(CredentialsKey, new Credentials(form.Email, hash), cancellationToken);
        User = await UserRepository.GetAsync(form.Email, hash, cancellationToken);
		LocalEventService.Send<LoggedIn>();
		GlobalEventService.Send(new PresenceChanged(User.IdTyped));

		return User != null;
	}
}
