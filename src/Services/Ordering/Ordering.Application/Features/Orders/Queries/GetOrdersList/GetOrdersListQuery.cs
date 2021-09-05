namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    using MediatR;
    using System.Collections.Generic;

    public class GetOrdersListQuery : IRequest<List<OrdersVm>>
    {
        public GetOrdersListQuery(string userName)
        {
            this.UserName = userName;
        }

        public string UserName { get; set; }
    }
}
