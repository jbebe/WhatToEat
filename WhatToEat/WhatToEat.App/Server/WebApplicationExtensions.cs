using WhatToEat.App.Storage.Repositories;

namespace WhatToEat.App.Server
{
    public static class WebApplicationExtensions
    {
        public static void InitDummyDatabase(this WebApplication app)
        {
            var users = app.Services.GetRequiredService<UserRepository>();
            users.Create("Admin");
            users.Create("User A");
            users.Create("User B");
        }
    }
}
