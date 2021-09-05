namespace Ordering.Infrastructure.Persistence
{
    using Microsoft.Extensions.Logging;
    using Ordering.Domain.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger, CancellationToken cancellationToken = default)
        {
            if (!orderContext.Orders.Any())
            {
                await orderContext.Orders.AddRangeAsync(GetPreconfiguredOrders(), cancellationToken);
                await orderContext.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order() { UserName = "cheng", FirstName = "Cheng", LastName = "Naja", EmailAddress = "cheng.worawit@gmail.com", AddressLine = "1234/4321", Country = "Thailand", TotalPrice = 350 }
            };
        }
    }
}
