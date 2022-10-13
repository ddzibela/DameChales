using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DameChales.API.DAL.Common.Entities;
using DameChales.API.DAL.Common.Repositories;

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
