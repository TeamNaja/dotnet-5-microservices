namespace Catalog.API.Repositories
{
    using Catalog.API.Entities;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken = default);

        Task<Product> GetProductByIdAsync(string id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Product>> GetProductsByNameAsync(string name, CancellationToken cancellationToken = default);

        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName, CancellationToken cancellationToken = default);

        Task CreateProductAsync(Product product, CancellationToken cancellationToken = default);

        Task<bool> UpdateProductAsync(Product product, CancellationToken cancellationToken = default);

        Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken = default);
    }
}
