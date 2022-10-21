using AutoMapper;
using DameChales.API.DAL.Common.Entities;
using DameChales.Common.Extensions;
using DameChales.Common.Models;

namespace DameChales.API.BL.MapperProfiles
{
    public class RestaurantMapperProfile : Profile
    {
        public RestaurantMapperProfile()
        {
            CreateMap<RestaurantEntity, RestaurantListModel>();

            CreateMap<RestaurantEntity, RestaurantDetailModel>()
                .MapMember(dst => dst.Foods, src => src.Foods);
            CreateMap<RestaurantDetailModel, RestaurantEntity>()
                .Ignore(dst => dst.Foods);

            CreateMap<RestaurantEntity, RestaurantDetailModel>()
                .MapMember(dst => dst.Orders, src => src.Orders);;
            CreateMap<RestaurantDetailModel, RestaurantEntity>()
                .Ignore(dst => dst.Orders);
        }
    }
}
