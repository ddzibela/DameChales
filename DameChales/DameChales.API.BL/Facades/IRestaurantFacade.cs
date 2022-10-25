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
        List<RestaurantListModel>? GetByName(string name);
        List<RestaurantListModel>? GetByAddress(string address);
        double GetEarnings(Guid id);
        Guid CreateOrUpdate(RestaurantDetailModel restaurantModel);
        Guid Create(RestaurantDetailModel restaurantModel);
        Guid? Update(RestaurantDetailModel restaurantModel);
        void Delete(Guid id);
    }
}
