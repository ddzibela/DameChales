using AutoMapper;
using DameChales.Common.Models;

namespace DameChales.Web.BL.MapperProfiles
{
    public class FoodMapperProfile : Profile
    {
        public FoodMapperProfile()
        {
            CreateMap<FoodDetailModel, FoodListModel>();
        }
    }
}