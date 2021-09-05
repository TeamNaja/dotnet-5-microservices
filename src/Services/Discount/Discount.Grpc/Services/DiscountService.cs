namespace Discount.Grpc.Services
{
    using global::Grpc.Core;
    using AutoMapper;
    using Discount.Entity.Entities;
    using Discount.Entity.Repositories;
    using Discount.Grpc.Protos;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository discountRepository;
        private readonly IMapper mapper;
        private readonly ILogger<DiscountService> logger;

        public DiscountService(
            IDiscountRepository discountRepository,
            IMapper mapper,
            ILogger<DiscountService> logger)
        {
            this.discountRepository = discountRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await discountRepository.GetDiscountAsync(request.ProductName);

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
            }

            logger.LogInformation(
                "Discount is retrieved for ProductName: {productName}, Amount: {Amount}, Description: {Description}", 
                coupon.ProductName, coupon.Amount, coupon.Description);

            return mapper.Map<CouponModel>(coupon);
        }

        public async override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = mapper.Map<Coupon>(request.Coupon);

            await discountRepository.CreateDiscountAsync(coupon);
            logger.LogInformation("Discount is successfully created. ProductName: {ProductName}", coupon.ProductName);

            return request.Coupon;
        }

        public async override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = mapper.Map<Coupon>(request.Coupon);

            await discountRepository.UpdateDiscountAsync(coupon);
            logger.LogInformation("Discount is successfully updated. ProductName: {ProductName}", coupon.ProductName);

            return request.Coupon;
        }

        public async override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await discountRepository.DeleteDiscountAsync(request.ProductName);

            return new DeleteDiscountResponse()
            {
                Success = deleted
            };
        }
    }
}
