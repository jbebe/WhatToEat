using Microsoft.EntityFrameworkCore;
using WhatToEat.App.Common;
using WhatToEat.App.Storage.Model;

namespace WhatToEat.App.Storage.Repositories
{
    public class UserRepository: RepositoryBase<User>
    {
        public UserRepository(StorageContext context): base(context) { }

        public async Task<User> CreateAsync(string name, CancellationToken cancellationToken)
        {
            var user = new User { 
                Id = ModelHelpers.GenerateId(), 
                Name = name 
            };
			await AddAsync(user, cancellationToken);
            
            return user;
		}

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await Context.Users.ToListAsync(cancellationToken);
        }
    }
}
