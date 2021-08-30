namespace Discount.API.Controllers
{
    using Discount.Entity.Entities;
    using Discount.Entity.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository discountRepository;
        private readonly ILogger<DiscountController> logger;

        public DiscountController(
            IDiscountRepository discountRepository,
            ILogger<DiscountController> logger)
        {
            this.discountRepository = discountRepository;
            this.logger = logger;
        }

        [HttpGet("{productName}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName, CancellationToken cancellationToken = default)
        {
            var coupon = await discountRepository.GetDiscountAsync(productName, cancellationToken);

            return this.Ok(coupon);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon, CancellationToken cancellationToken = default)
        {
            await discountRepository.CreateDiscountAsync(coupon, cancellationToken);

            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateDiscount([FromBody] Coupon coupon, CancellationToken cancellationToken = default)
        {
            return this.Ok(await discountRepository.UpdateDiscountAsync(coupon, cancellationToken));
        }

        [HttpDelete("{productName}", Name = "DeleteDiscount")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteDiscount(string productName, CancellationToken cancellationToken = default)
        {
            return this.Ok(await discountRepository.DeleteDiscountAsync(productName, cancellationToken));
        }
    }
}
