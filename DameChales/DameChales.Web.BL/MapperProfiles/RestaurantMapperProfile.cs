using AutoMapper;
using DameChales.Common.Models;

namespace DameChales.Web.BL.MapperProfiles
{
    public class RestaurantMapperProfile : Profile
    {
        public RestaurantMapperProfile()
        {
            CreateMap<RestaurantDetailModel, RestaurantListModel>();
        }
    }
}