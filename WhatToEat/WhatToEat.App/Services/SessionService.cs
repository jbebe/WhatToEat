using WhatToEat.App.Common;
using WhatToEat.App.Services.Models;
using WhatToEat.App.Storage.Model;
using WhatToEat.App.Storage.Repositories;

namespace WhatToEat.App.Services;

public class SessionService
{
	public User? User { get; set; } = default!;
	
	private UserRepository UserRepository { get; }

	public SessionService(UserRepository userRepository)
	{
		UserRepository = userRepository;
	}

	public async Task<bool> LoginAsync(LoginForm form, CancellationToken cancellationToken)
	{
		User = await UserRepository.GetAsync(
			form.Email, ModelHelpers.GetPasswordHash(form.Password), cancellationToken);

		return User != null;
	}
}
