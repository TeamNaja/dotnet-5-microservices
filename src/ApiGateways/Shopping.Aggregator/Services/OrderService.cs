namespace Shopping.Aggregator.Services
{
    using Shopping.Aggregator.Extensions;
    using Shopping.Aggregator.Models;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class OrderService : IOrderService
    {
        private readonly HttpClient client;

        public OrderService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName, CancellationToken cancellationToken = default)
        {
            var response = await client.GetAsync($"/api/v1/Order/{userName}", cancellationToken);

            return await response.ReadContentAs<List<OrderResponseModel>>(cancellationToken);
        }
    }
}
