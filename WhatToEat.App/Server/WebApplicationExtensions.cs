using WhatToEat.App.Common;
using WhatToEat.App.Storage.Dtos;
using WhatToEat.App.Storage.Model;
using WhatToEat.App.Storage.Repositories;

namespace WhatToEat.App.Server
{
    public static class WebApplicationExtensions
    {
        public static async Task InitDummyDatabaseAsync(this WebApplication app)
        {
            // Create new entities
            using var scope = app.Services.CreateScope();
            var ct = CancellationToken.None;

            // Create users
            var userRepo = scope.ServiceProvider.GetRequiredService<UserRepository>();
            var users = await userRepo.GetAllAsync(null, ct);
            if (users.Count >= 3)
            {
                // Already initialized with dummy data
                return;
            }
            var admin = await userRepo.CreateAsync(new CreateUser("Admin", "admin@example.com", "test", Admin: true), ct);
            var userA = await userRepo.CreateAsync(new CreateUser("User A", "user_a@example.com", "test"), ct);
            var userB = await userRepo.CreateAsync(new CreateUser("User B", "user_b@example.com", "test"), ct);

            // Create restaurants
            var restaurantRepo = scope.ServiceProvider.GetRequiredService<RestaurantRepository>();
            var restaurantA = await restaurantRepo.CreateAsync("Restaurant A", new[] { PaymentMethod.Cash }, new[] { ConsumptionType.DineIn, ConsumptionType.Takeaway }, ct);
            var restaurantB = await restaurantRepo.CreateAsync("Restaurant B", new[] { PaymentMethod.Cash, PaymentMethod.BankCard }, new[] { ConsumptionType.DineIn, ConsumptionType.Takeaway, ConsumptionType.Delivery }, ct);
            var restaurantC = await restaurantRepo.CreateAsync("Restaurant C", new[] { PaymentMethod.Cash, PaymentMethod.BankCard, PaymentMethod.SzepCard }, new[] { ConsumptionType.Delivery }, ct);
            var restaurantD = await restaurantRepo.CreateAsync("Restaurant D", new[] { PaymentMethod.BankCard }, new[] { ConsumptionType.Takeaway, ConsumptionType.Delivery }, ct);

            // Create votes
            var voteRepo = scope.ServiceProvider.GetRequiredService<VoteRepository>();
            await voteRepo.CreateOrUpdateAsync(userA.IdTyped, new List<Restaurant>{ restaurantA }, ct);
            await voteRepo.CreateOrUpdateAsync(userB.IdTyped, new List<Restaurant> { restaurantA }, ct);
            await voteRepo.CreateOrUpdateAsync(userA.IdTyped, new List<Restaurant> { restaurantB }, ct, DateTime.UtcNow.AddDays(-1).Date);
            await voteRepo.CreateOrUpdateAsync(userB.IdTyped, new List<Restaurant> { restaurantB }, ct, DateTime.UtcNow.AddDays(-1).Date);
        }
    }
}
