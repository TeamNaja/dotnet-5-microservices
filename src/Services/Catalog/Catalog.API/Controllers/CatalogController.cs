namespace Catalog.API.Controllers
{
    using Catalog.API.Entities;
    using Catalog.API.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ILogger<CatalogController> logger;

        public CatalogController(
            IProductRepository productRepository,
            ILogger<CatalogController> logger)
        {
            this.productRepository = productRepository;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(CancellationToken cancellationToken = default)
        {
            var products = await productRepository.GetProductsAsync(cancellationToken);

            return this.Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id, CancellationToken cancellationToken = default)
        {
            var product = await productRepository.GetProductByIdAsync(id, cancellationToken);

            if(product is null)
            {
                logger.LogError($"Product with id: {id}, not found.");
                return this.NotFound();
            }

            return this.Ok(product);
        }

        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category, CancellationToken cancellationToken = default)
        {
            var products = await productRepository.GetProductsByCategoryAsync(category, cancellationToken);

            return this.Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product, CancellationToken cancellationToken = default)
        {
            await productRepository.CreateProductAsync(product, cancellationToken);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product, CancellationToken cancellationToken = default)
        {
            return this.Ok(await productRepository.UpdateProductAsync(product, cancellationToken));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id, CancellationToken cancellationToken = default)
        {
            return this.Ok(await productRepository.DeleteProductAsync(id, cancellationToken));
        }
    }
}
