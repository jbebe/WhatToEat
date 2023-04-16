using WhatToEat.App.Common;
using WhatToEat.App.Storage.Model;

namespace WhatToEat.App.Storage.Repositories
{
    public class UserRepository
    {
        public StorageContext Context { get; }

        public UserRepository(StorageContext context)
        {
            Context = context;
        }

        public void Create(string name)
        {
            Context.Users.Add(new User { Id = ModelHelpers.GenerateId(), Name = name });
        }

        public IEnumerable<User> GetAll()
        {
            return Context.Users;
        }
    }
}
