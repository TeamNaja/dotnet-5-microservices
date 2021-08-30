namespace Basket.API.Repositories
{
    using Basket.API.Entities;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken = default);

        Task<ShoppingCart> UpdateBasketAsync(ShoppingCart shoppingCart, CancellationToken cancellationToken = default);

        Task DeleteBasketAsync(string userName, CancellationToken cancellationToken = default);
    }
}
