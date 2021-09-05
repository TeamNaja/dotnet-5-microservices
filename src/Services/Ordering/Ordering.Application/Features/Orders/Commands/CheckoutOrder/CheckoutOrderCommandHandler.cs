namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    using AutoMapper;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Ordering.Application.Contracts.Infrastructure;
    using Ordering.Application.Contracts.Persistence;
    using Ordering.Application.Models;
    using Ordering.Domain.Entities;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly IEmailService emailServices;
        private readonly ILogger<CheckoutOrderCommandHandler> logger;

        public CheckoutOrderCommandHandler(
            IOrderRepository orderRepository, 
            IMapper mapper, 
            IEmailService emailServices,
            ILogger<CheckoutOrderCommandHandler> logger)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.emailServices = emailServices;
            this.logger = logger;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken = default)
        {
            var orderEntity = mapper.Map<Order>(request);
            var entity = await orderRepository.AddAsync(orderEntity, cancellationToken);

            logger.LogInformation($"Order { entity.Id } is successfully created.");

            await SendEmail(entity, cancellationToken);

            return entity.Id;
        }

        private async Task SendEmail(Order order, CancellationToken cancellationToken = default)
        {
            var email = new Email() { To = "cheng.worawit@gmail.com", Body = $"Order was created.", Subject = $"Order { order.Id } was created." };

            try
            {
                await emailServices.SendEmailAsync(email, cancellationToken);
            } 
            catch (Exception ex)
            {
                logger.LogError($"Order { order.Id } failed due to an error with email service: { ex.Message }");
            }
        }
    }
}
