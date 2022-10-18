using System;
using System.Collections.Generic;
using DameChales.Common.BL.Facades;
using DameChales.Common.Models;

namespace DameChales.API.BL.Facades
{
    public interface IFoodFacade : IAppFacade
    {
        List<FoodListModel> GetAll();
        FoodDetailModel? GetById(Guid id);

        // TODO ziskanie jedal pre restauraciu
        // TODO ziskanie jedla byName atd..
        Guid CreateOrUpdate(FoodDetailModel foodModel);
        Guid Create(FoodDetailModel foodModel);
        Guid? Update(FoodDetailModel foodModel);
        void Delete(Guid id);
    }
}
