namespace Shopping.Aggregator.Services
{
    using Shopping.Aggregator.Extensions;
    using Shopping.Aggregator.Models;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class CatalogService : ICatalogService
    {
        private readonly HttpClient client;

        public CatalogService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<CatalogModel> GetCatalog(string id, CancellationToken cancellationToken = default)
        {
            var response = await client.GetAsync($"/api/v1/Catalog/{id}", cancellationToken);

            return await response.ReadContentAs<CatalogModel>(cancellationToken);
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogs(CancellationToken cancellationToken = default)
        {
            var response = await client.GetAsync("/api/v1/Catalog", cancellationToken);

            return await response.ReadContentAs<List<CatalogModel>>(cancellationToken);
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogsByCategory(string category, CancellationToken cancellationToken = default)
        {
            var response = await client.GetAsync($"/api/v1/Catalog/GetProductByCategory/{category}", cancellationToken);

            return await response.ReadContentAs<List<CatalogModel>>(cancellationToken);
        }
    }
}
