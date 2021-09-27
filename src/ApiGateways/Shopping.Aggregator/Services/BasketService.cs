namespace Shopping.Aggregator.Services
{
    using Shopping.Aggregator.Extensions;
    using Shopping.Aggregator.Models;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class BasketService : IBasketService
    {
        private readonly HttpClient client;

        public BasketService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<BasketModel> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var response = await client.GetAsync($"/api/v1/Basket/{userName}", cancellationToken);

            return await response.ReadContentAs<BasketModel>(cancellationToken);
        }
    }
}
