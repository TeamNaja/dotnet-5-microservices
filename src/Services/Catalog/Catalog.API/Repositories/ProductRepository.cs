namespace Catalog.API.Repositories
{
    using Catalog.API.Data;
    using Catalog.API.Entities;
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            this.catalogContext = catalogContext;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken = default)
        {
            return await catalogContext.Products
                .Find(x => true)
                .ToListAsync(cancellationToken);
        }

        public async Task<Product> GetProductByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await catalogContext.Products
                .Find(x => x.Id == id)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var filterDefinition = Builders<Product>.Filter.Eq(x => x.Name, name);

            return await catalogContext.Products
                .Find(filterDefinition)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName, CancellationToken cancellationToken = default)
        {
            var filterDefinition = Builders<Product>.Filter.Eq(x => x.Category, categoryName);

            return await catalogContext.Products
                .Find(filterDefinition)
                .ToListAsync(cancellationToken);
        }

        public async Task CreateProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            await catalogContext.Products.InsertOneAsync(product, cancellationToken: cancellationToken);
        }

        public async Task<bool> UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            var updateResult = await catalogContext.Products
                .ReplaceOneAsync(
                    filter: x => x.Id == product.Id, 
                    replacement: product, 
                    cancellationToken: cancellationToken);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken = default)
        {
            var filterDefination = Builders<Product>.Filter.Eq(x => x.Id, id);
            var deleteResult = await catalogContext.Products.DeleteOneAsync(filterDefination, cancellationToken);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
