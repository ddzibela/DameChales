using AutoMapper;
using DameChales.API.DAL.Common.Entities;
using DameChales.Common.Extensions;
using DameChales.Common.Models;

namespace DameChales.API.BL.MapperProfiles
{
    public class OrderMapperProfile : Profile
    {
        public OrderMapperProfile()
        {
            CreateMap<OrderEntity, OrderListModel>();
            CreateMap<OrderEntity, OrderDetailModel>()
                .MapMember(dst => dst.FoodAmounts, src => src.FoodAmounts);
            CreateMap<FoodAmountEntity, OrderDetailFoodModel>();

            CreateMap<OrderDetailModel, OrderEntity>()
                .Ignore(dst => dst.FoodAmounts);
        }
    }
}
