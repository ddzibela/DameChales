using System;
using System.Collections.Generic;
using System.Linq;
using DameChales.API.DAL.Common.Entities;
using DameChales.API.DAL.Common.Repositories;

namespace DameChales.API.DAL.Memory.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IList<OrderEntity> orders;
        private readonly IList<FoodAmountEntity> foodAmounts;
        private readonly IList<FoodEntity> foods;

        public OrderRepository(
            Storage storage)
        {
            this.orders = storage.Orders;
            this.foodAmounts = storage.FoodAmounts;
            this.foods = storage.Foods;
        }

        public IList<OrderEntity> GetAll()
        {
            return this.orders;
        }

        public OrderEntity? GetById(Guid id)
        {
            var orderEntity = orders.SingleOrDefault(order => order.Id == id);

            if (orderEntity is not null)
            {
                orderEntity.FoodAmounts = GetFoodAmountsByOrderId(id);
                foreach (var foodAmount in orderEntity.FoodAmounts)
                {
                    foodAmount.Food = foods.SingleOrDefault(foodEntity => foodEntity.Id == foodAmount.FoodId);
                }
            }

            return orderEntity;
        }

        public Guid Insert(OrderEntity entity) //todo
        {
            orders.Add(entity);

            foreach (var foodAmount in entity.FoodAmounts)
            {
                foodAmounts.Add(new FoodAmountEntity
                {
                    Id = ingredientAmount.Id,




                    Amount = ingredientAmount.Amount,
                    Unit = ingredientAmount.Unit,
                    RecipeId = entity.Id,
                    IngredientId = ingredientAmount.IngredientId
                });
            }

            return entity.Id;
        }

        public Guid? Update(OrderEntity entity)
        {
            var orderEntityExisting = orders.SingleOrDefault(orderEntity => orderEntity.Id == entity.Id);

            if (orderEntityExisting is not null)
            {
                orderEntityExisting.IngredientAmounts = GetIngredientAmountsByOrderId(entity.Id);
                UpdateOrderAmounts(entity, orderEntityExisting);
                return orderEntityExisting.Id;
            }
            else
            {
                return null;
            }
        }


    }
}
