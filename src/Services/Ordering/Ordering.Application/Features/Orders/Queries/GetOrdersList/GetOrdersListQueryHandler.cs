namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    using AutoMapper;
    using MediatR;
    using Ordering.Application.Contracts.Persistence;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrdersVm>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        public async Task<List<OrdersVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken = default)
        {
            var orderList = await orderRepository.GetOrdersByUserName(request.UserName, cancellationToken);

            return mapper.Map<List<OrdersVm>>(orderList);
        }
    }
}
