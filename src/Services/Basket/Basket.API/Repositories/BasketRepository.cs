namespace Basket.API.Repositories
{
    using Basket.API.Entities;
    using Microsoft.Extensions.Caching.Distributed;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache distributedCache;

        public BasketRepository(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await distributedCache.GetStringAsync(userName, cancellationToken);

            if (string.IsNullOrWhiteSpace(basket))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
        {
            await distributedCache.SetStringAsync(shoppingCart.UserName, JsonConvert.SerializeObject(shoppingCart), cancellationToken);

            return await GetBasketAsync(shoppingCart.UserName);
        }

        public async Task DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            await distributedCache.RemoveAsync(userName, cancellationToken);
        }
    }
}
