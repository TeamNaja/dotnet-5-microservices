namespace Basket.API.Controllers
{
    using Basket.API.Entities;
    using Basket.API.GrpcServices;
    using Basket.API.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly DiscountGrpcService discountGrpcService;
        private readonly ILogger<BasketController> logger;


        public BasketController(
            IBasketRepository basketRepository,
            DiscountGrpcService discountGrpcService,
            ILogger<BasketController> logger)
        {
            this.basketRepository = basketRepository;
            this.discountGrpcService = discountGrpcService;
            this.logger = logger;
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await basketRepository.GetBasketAsync(userName, cancellationToken);

            return this.Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
        {
            // TODO : Communicate with Discount.Grpc and calculate latest price of product into shopping cart
            // consume discount Grpc
            foreach(var item in shoppingCart.Items)
            {
                var coupon = await discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            return this.Ok(await basketRepository.UpdateBasketAsync(shoppingCart, cancellationToken));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await basketRepository.DeleteBasketAsync(userName, cancellationToken);

            return this.Ok();
        }
    }
}
