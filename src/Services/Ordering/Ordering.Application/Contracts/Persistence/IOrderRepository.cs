namespace Ordering.Application.Contracts.Persistence
{
    using Ordering.Domain.Entities;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IOrderRepository: IAsyncRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserName(string userName, CancellationToken cancellationToken = default);
    }
}
