namespace Discount.Grpc.Mapper
{
    using AutoMapper;
    using Discount.Entity.Entities;
    using Discount.Grpc.Protos;

    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
