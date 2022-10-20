using System;
using System.Collections.Generic;
using DameChales.Common.BL.Facades;
using DameChales.Common.Models;

namespace DameChales.API.BL.Facades
{
    public interface IRestaurantFacade : IAppFacade
    {
        List<RestaurantListModel> GetAll();
        List<RestaurantListModel> GetByFoodId(Guid id);
        RestaurantDetailModel? GetById(Guid id);
        RestaurantDetailModel? GetByName(string name);
        RestaurantDetailModel? GetByAddress(string address);
        Guid CreateOrUpdate(RestaurantDetailModel restaurantModel);
        Guid Create(RestaurantDetailModel restaurantModel);
        Guid? Update(RestaurantDetailModel restaurantModel);
        void Delete(Guid id);
    }
}
