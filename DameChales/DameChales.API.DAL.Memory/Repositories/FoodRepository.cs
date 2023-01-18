using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using DameChales.API.DAL.Common.Entities;
using DameChales.API.DAL.Common.Repositories;
using DameChales.Common.Enums;

namespace DameChales.API.DAL.Memory.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private readonly IList<FoodEntity> foods;
        private readonly IList<FoodAmountEntity> foodAmounts;
        private readonly IMapper mapper;

        public FoodRepository(
            Storage storage,
            IMapper mapper)
        {
            this.foods = storage.Foods;
            this.foodAmounts = storage.FoodAmounts;
            this.mapper = mapper;
        }

        public IList<FoodEntity> GetAll()
        {
            return foods;
        }

        public FoodEntity? GetById(Guid id)
        {
            return foods.SingleOrDefault(entity => entity.Id == id);
        }

        public IList<FoodEntity> GetByRestaurantId(Guid id)
        {
            return foods.Where(entity => entity.RestaurantGuid == id).ToList();
        }

        public IList<FoodEntity> GetByName(Guid id, string name)
        {   
            var restaurantFoods = GetByRestaurantId(id);
            var nameRegex = new Regex(name);
            return restaurantFoods.Where(e => nameRegex.IsMatch(e.Name)).ToList();
        }

        /// <summary>
        /// Returns list of foods matching name regex
        /// </summary>
        public IList<FoodEntity> GetByName(string name)
        {
            var nameRegex = new Regex(name);
            return foods.Where(e => nameRegex.IsMatch(e.Name)).ToList();
        }

        public IList<FoodEntity> GetByDescription(Guid id, string name)
        {
            var restaurantFoods = GetByRestaurantId(id);
            var nameRegex = new Regex(name);
            return restaurantFoods.Where(e => nameRegex.IsMatch(e.Description)).ToList();
        }
        public IList<FoodEntity> GetByDescription(string name)
        {
            var nameRegex = new Regex(name);
            return foods.Where(e => nameRegex.IsMatch(e.Description)).ToList();
        }

        /// <summary>
        /// get all foods from one restaurant not containing select alergens
        /// </summary>
        /// <param name="id">Restaurant GUID</param>
        /// <param name="alergens">List of alergens to exclude</param>
        /// <returns>List of foods without alergens</returns>
        public IList<FoodEntity> GetWithoutAlergens(Guid id, HashSet<Alergens> alergens)
        {
            var restaurantFoods = GetByRestaurantId(id);
            return restaurantFoods.Where(e => e.alergens.Intersect(alergens).Any() == false).ToList();
        }

        public IList<FoodEntity> GetWithoutAlergens(HashSet<Alergens> alergens)
        {
            var restaurantFoods = GetAll();
            return restaurantFoods.Where(e => e.alergens.Intersect(alergens).Any() == false).ToList();
        }

        public Guid Insert(FoodEntity food)
        {
            foods.Add(food);
            return food.Id;
        }

        public Guid? Update(FoodEntity entity)
        {
            var foodExisting = foods.SingleOrDefault(foodInStorage =>
                foodInStorage.Id == entity.Id);
            if (foodExisting != null)
            {
                mapper.Map(entity, foodExisting);
            }

            return foodExisting?.Id;
        }

        public void Remove(Guid id)
        {
            var foodAmountsToRemove =
                foodAmounts.Where(foodAmount => foodAmount.FoodGuid == id).ToList();

            for (var i = 0; i < foodAmountsToRemove.Count; i++)
            {
                var foodAmountToRemove = foodAmountsToRemove.ElementAt(i);
                foodAmounts.Remove(foodAmountToRemove);
            }

            var foodToRemove = foods.Single(food => food.Id.Equals(id));
            foods.Remove(foodToRemove);
        }

        public bool Exists(Guid id)
        {
            return foods.Any(food => food.Id == id);
        }
    }
}
