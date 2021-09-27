namespace Shopping.Aggregator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Shopping.Aggregator.Models;
    using Shopping.Aggregator.Services;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService catalogService;
        private readonly IBasketService basketService;
        private readonly IOrderService orderService;

        public ShoppingController(
            ICatalogService catalogService,
            IBasketService basketService,
            IOrderService orderService)
        {
            this.catalogService = catalogService;
            this.basketService = basketService;
            this.orderService = orderService;
        }

        [HttpGet("{userName}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName, CancellationToken cancellationToken = default)
        {
            // TODO naja
            // get basket with username
            // iterate basket items and consume products with basket item productId member
            // map product related members into basketitem dto with extended columns
            // consume ordering microservices in order to retrieve order list
            // return root ShoppngModel dto class which including all responses

            var basket = await basketService.GetBasket(userName, cancellationToken);

            foreach(var item in basket.Items)
            {
                var product = await catalogService.GetCatalog(item.ProductId, cancellationToken);

                // set additional product fields onto basket item
                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }

            var order = await orderService.GetOrdersByUserName(userName, cancellationToken);

            return this.Ok(new ShoppingModel()
            {
                UserName = userName,
                BasketWithProducts = basket,
                Orders = order
            });
        }
    }
}
