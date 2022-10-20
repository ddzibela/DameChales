﻿using DameChales.API.DAL.Common.Entities;
using DameChales.API.DAL.Common.Entities.Interfaces;
using DameChales.Common.Enums;
using DameChales.Common.Models;

namespace DameChales.API.DAL.Common.Repositories
{
    public interface IFoodRepository : IApiRepository<FoodEntity>
    {
        IList<FoodEntity> GetByRestaurantId(Guid id);
        IList<FoodEntity> GetWithoutAlergens(HashSet<Alergens> alergens);
        IList<FoodEntity> GetByName(string name);
    }
}
