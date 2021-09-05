namespace Ordering.API.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
    using Ordering.Application.Features.Orders.Commands.DeleteOrder;
    using Ordering.Application.Features.Orders.Commands.UpdateOrder;
    using Ordering.Application.Features.Orders.Queries.GetOrdersList;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{userName}", Name = "GetOrder")]
        [ProducesResponseType(typeof(IEnumerable<OrdersVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrdersVm>>> GetOrderByUserName(string userName, CancellationToken cancellationToken = default)
        {
            var query = new GetOrdersListQuery(userName);
            var orders = await mediator.Send(query, cancellationToken);

            return this.Ok(orders);
        }

        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command, CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(command, cancellationToken);

            return this.Ok(result);
        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command, CancellationToken cancellationToken = default)
        {
            await mediator.Send(command, cancellationToken);

            return this.NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteOrder(int id, CancellationToken cancellationToken = default)
        {
            var command = new DeleteOrderCommand() { Id = id };
            await mediator.Send(command, cancellationToken);

            return this.NoContent();
        }
    }
}
