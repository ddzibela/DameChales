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
            CreateMap<RestaurantEntity, RestaurantListModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.PhotoURL, opt => opt.MapFrom(src => src.LogoURL))
                .ReverseMap();

            CreateMap<RestaurantEntity, RestaurantDetailModel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.PhotoURL, opt => opt.MapFrom(src => src.LogoURL))
                .ForMember(dst => dst.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dst => dst.GPSCoordinates, opt => opt.MapFrom(src => src.GPSCoordinates))
                .ForMember(dst => dst.Foods, opt => opt.MapFrom(src => src.Foods))
                .ForMember(dst => dst.Orders, opt => opt.MapFrom(src => src.Orders))
                .ReverseMap();
        }
    }
}
