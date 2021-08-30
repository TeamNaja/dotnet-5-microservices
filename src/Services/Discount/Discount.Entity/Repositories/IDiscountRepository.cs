namespace Discount.Entity.Repositories
{
    using Discount.Entity.Entities;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscountAsync(string productName, CancellationToken cancellationToken = default);

        Task<bool> CreateDiscountAsync(Coupon coupon, CancellationToken cancellationToken = default);

        Task<bool> UpdateDiscountAsync(Coupon coupon, CancellationToken cancellationToken = default);

        Task<bool> DeleteDiscountAsync(string productName, CancellationToken cancellationToken = default);
    }
}
