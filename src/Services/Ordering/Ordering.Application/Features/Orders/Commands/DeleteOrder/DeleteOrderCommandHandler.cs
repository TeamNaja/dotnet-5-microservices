namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Ordering.Application.Contracts.Persistence;
    using Ordering.Application.Exceptions;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository orderRepository;
        private readonly ILogger<DeleteOrderCommandHandler> logger;

        public DeleteOrderCommandHandler(
            IOrderRepository orderRepository, 
            ILogger<DeleteOrderCommandHandler> logger)
        {
            this.orderRepository = orderRepository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken = default)
        {
            var entity = await orderRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity is null)
            {
                logger.LogError($"Order { request.Id} does not exist.");

                throw new NotFoundException(nameof(entity), request.Id);
            }

            await orderRepository.DeleteAsync(entity, cancellationToken);

            logger.LogInformation($"Order { entity.Id } is successfully deleted.");

            return Unit.Value;
        }
    }
}
