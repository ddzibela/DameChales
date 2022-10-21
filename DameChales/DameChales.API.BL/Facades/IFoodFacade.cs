using System;
using System.Collections.Generic;
using DameChales.Common.BL.Facades;
using DameChales.Common.Enums;
using DameChales.Common.Models;

namespace DameChales.API.BL.Facades
{
    public interface IFoodFacade : IAppFacade
    {
        List<FoodListModel> GetAll();
        FoodDetailModel? GetById(Guid id);
        List<FoodListModel> GetByRestaurantId(Guid id);
        List<FoodListModel> GetByName(string name);
        List<FoodListModel> GetWithoutAlergens(Guid restaurantId, HashSet<Alergens> alergens);
        Guid CreateOrUpdate(FoodDetailModel foodModel);
        Guid Create(FoodDetailModel foodModel);
        Guid? Update(FoodDetailModel foodModel);
        void Delete(Guid id);
    }
}
