using System.Security.Cryptography;
using System.Text;
using WhatToEat.App.Common;
using WhatToEat.App.Storage.Dtos;
using WhatToEat.App.Storage.Model;

namespace WhatToEat.App.Storage.Repositories
{
	public class UserRepository: RepositoryBase<User>
    {
        public UserRepository(StorageContext context): base(context) { }

        public async Task<User> CreateAsync(CreateUser createUser, CancellationToken cancellationToken)
        {
            var user = new User { 
                Id = new Id<User>(),
                Name = createUser.Name,
                Email = createUser.Email,
                PasswordHash = ModelHelpers.GetPasswordHash(createUser.Password),
                Admin = createUser.Admin,
            };
			await CreateOrUpdateAsync(user, x => x.Id == user.Id, null, cancellationToken);
            
            return user;
		}

        public async Task<User?> GetAsync(string email, string hash, CancellationToken cancellationToken)
        {
            var users = await QueryAsync(user => user.Email == email && user.PasswordHash == hash, null, cancellationToken);
            return users.SingleOrDefault();
		}
    }
}
