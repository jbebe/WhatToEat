using WhatToEat.App.Services.Models;
using WhatToEat.App.Storage.Model;
using WhatToEat.App.Storage.Repositories;

namespace WhatToEat.App.Services;

public class SessionService
{
	public User User { get; set; } = default!;
	
	private UserRepository UserRepository { get; }

	public SessionService(UserRepository userRepository)
	{
		UserRepository = userRepository;
	}

	public async Task LoginAsync(LoginForm form, CancellationToken cancellationToken)
	{
		var users = await UserRepository.GetAllAsync(cancellationToken);
		User = users.First(x => x.Admin);
	}
}
