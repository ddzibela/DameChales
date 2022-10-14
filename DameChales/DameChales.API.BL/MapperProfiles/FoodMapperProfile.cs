using AutoMapper;
using DameChales.API.DAL.Common.Entities;
using DameChales.Common.Models;

namespace DameChales.API.BL.MapperProfiles
{
    public class FoodMapperProfile : Profile
    {
        public FoodMapperProfile()
        {
            CreateMap<IngredientEntity, IngredientListModel>();
            CreateMap<IngredientEntity, IngredientDetailModel>();

            CreateMap<IngredientDetailModel, IngredientEntity>();
        }
    }
}
