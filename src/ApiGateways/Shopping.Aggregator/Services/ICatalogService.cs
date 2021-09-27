namespace Shopping.Aggregator.Services
{
    using Shopping.Aggregator.Models;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ICatalogService
    {
        Task<IEnumerable<CatalogModel>> GetCatalogs(CancellationToken cancellationToken = default);

        Task<IEnumerable<CatalogModel>> GetCatalogsByCategory(string category, CancellationToken cancellationToken = default);

        Task<CatalogModel> GetCatalog(string id, CancellationToken cancellationToken = default);
    }
}
