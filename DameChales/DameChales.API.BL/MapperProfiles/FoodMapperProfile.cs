using AutoMapper;
using DameChales.API.DAL.Common.Entities;
using DameChales.Common.Extensions;
using DameChales.Common.Models;

namespace DameChales.API.BL.MapperProfiles
{
    public class FoodMapperProfile : Profile
    {
        public FoodMapperProfile()
        {
            CreateMap<FoodEntity, FoodListModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.PhotoURL, opt => opt.MapFrom(src => src.PhotoURL))
                .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.Price))
                //.Ignore(dst => dst.RestaurantGuid)
                //.Ignore(dst => dst.Restaurant)
                //.Ignore(dst => dst.alergens)
                //.Ignore(dst => dst.Description)
                .ReverseMap();

            CreateMap<FoodEntity, FoodDetailModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.PhotoURL, opt => opt.MapFrom(src => src.PhotoURL))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dst => dst.alergens, opt => opt.MapFrom(src => src.alergens))
                .ForMember(dst => dst.RestaurantGuid, opt => opt.MapFrom(src => src.RestaurantGuid))
                .Ignore(dst => dst.Restaurant)
                .ReverseMap();
        }
    }
}
