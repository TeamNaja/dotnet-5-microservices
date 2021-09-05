namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    using AutoMapper;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Ordering.Application.Contracts.Persistence;
    using Ordering.Application.Exceptions;
    using Ordering.Domain.Entities;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateOrderCommandHandler> logger;

        public UpdateOrderCommandHandler(
            IOrderRepository orderRepository, 
            IMapper mapper, 
            ILogger<UpdateOrderCommandHandler> logger)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken = default)
        {
            var entity = await orderRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity is null)
            {
                logger.LogError($"Order { request.Id} does not exist.");

                throw new NotFoundException(nameof(entity), request.Id);
            }

            mapper.Map(request, entity, typeof(UpdateOrderCommand), typeof(Order));

            await orderRepository.UpdateAsync(entity, cancellationToken);

            logger.LogInformation($"Order { entity.Id } is successfully updated.");

            return Unit.Value;
        }
    }
}
