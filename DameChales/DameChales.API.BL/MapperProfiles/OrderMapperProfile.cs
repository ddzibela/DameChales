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
            CreateMap<OrderEntity, OrderListModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.RestaurantGuid, opt => opt.MapFrom(src => src.RestaurantGuid))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => src.Status))
                .ReverseMap();

            CreateMap<OrderEntity, OrderDetailModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.RestaurantGuid, opt => opt.MapFrom(src => src.RestaurantGuid))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Note, opt => opt.MapFrom(src => src.Note))
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dst => dst.FoodAmounts, opt => opt.MapFrom(src => src.FoodAmounts))
                .ReverseMap();

            CreateMap<FoodAmountEntity, OrderFoodAmountDetailModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Note, opt => opt.MapFrom(src => src.Note))
                .ForMember(dst => dst.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dst => dst.Food, opt => opt.MapFrom(src => src.FoodEntity))
                .ReverseMap();
        }
    }
}
