using System;
using System.Collections.Generic;
using System.Linq;
using DameChales.API.DAL.Common.Entities;
using DameChales.API.DAL.Common.Repositories;
using DameChales.Common.Enums;

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
                    foodAmount.FoodEntity = foods.SingleOrDefault(foodEntity => foodEntity.Id == foodAmount.FoodGuid);
                }
            }

            return orderEntity;
        }

        public IList<OrderEntity> GetByRestaurantId(Guid id)
        {
            return orders.Where(order => order.RestaurantGuid == id).ToList();
        }

        public IList<OrderEntity> GetByFoodId(Guid id)
        {
            return orders.Where(order => order.FoodAmounts.Any(amount => amount.FoodGuid == id)).ToList();
        }

        public IList<OrderEntity> GetByStatus(Guid restaurantId, OrderStatus status)
        {
            return orders.Where(order => order.RestaurantGuid == restaurantId && order.Status == status).ToList();
        }

        public Guid Insert(OrderEntity entity)
        {
            orders.Add(entity);

            foreach (var foodAmount in entity.FoodAmounts)
            {
                foodAmounts.Add(new FoodAmountEntity
                {
                    Id = foodAmount.Id,
                    FoodGuid = foodAmount.FoodGuid,
                    OrderGuid = foodAmount.OrderGuid,
                    Amount = foodAmount.Amount,
                    Note = foodAmount.Note
                });
            }

            return entity.Id;
        }

        public Guid? Update(OrderEntity entity)
        {
            var orderEntityExisting = orders.SingleOrDefault(orderEntity => orderEntity.Id == entity.Id);

            if (orderEntityExisting is not null)
            {
                orderEntityExisting.Status = entity.Status;
                orderEntityExisting.Name = entity.Name;
                orderEntityExisting.Address = entity.Address;
                orderEntityExisting.DeliveryTime = entity.DeliveryTime;
                orderEntityExisting.Note = entity.Note;
                orderEntityExisting.FoodAmounts = GetFoodAmountsByOrderId(entity.Id);
                UpdateFoodAmounts(entity, orderEntityExisting);
                return orderEntityExisting.Id;
            }
            else
            {
                return null;
            }
        }

        private void UpdateFoodAmounts(OrderEntity updatedEntity, OrderEntity existingEntity)
        {
            var foodAmountsToDelete = existingEntity.FoodAmounts.Where(t =>
                !updatedEntity.FoodAmounts.Select(a => a.Id).Contains(t.Id));
            DeleteFoodAmounts(foodAmountsToDelete);

            var orderUpdateFoodModelsToInsert = updatedEntity.FoodAmounts.Where(t =>
                !existingEntity.FoodAmounts.Select(a => a.Id).Contains(t.Id));
            InsertFoodAmounts(existingEntity, orderUpdateFoodModelsToInsert);

            var orderUpdateFoodModelsToUpdate = updatedEntity.FoodAmounts.Where(
                food => existingEntity.FoodAmounts.Any(ia => ia.FoodGuid == food.FoodGuid));
            UpdateFoodAmounts(existingEntity, orderUpdateFoodModelsToUpdate);
        }

        private void UpdateFoodAmounts(OrderEntity orderEntity,
            IEnumerable<FoodAmountEntity> orderFoodModelsToUpdate)
        {
            foreach (var orderUpdateFoodModel in orderFoodModelsToUpdate)
            {
                FoodAmountEntity? foodAmountEntity;
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
                if (orderUpdateFoodModel.Id == null)
                {
                    foodAmountEntity = GetFoodAmountOrderIdAndFoodId(orderEntity.Id,
                        orderUpdateFoodModel.FoodGuid);
                }
                else
                {
                    foodAmountEntity = foodAmounts.Single(t => (t.Id == orderUpdateFoodModel.Id && t.OrderGuid == orderUpdateFoodModel.OrderGuid));
                }
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'

                if (foodAmountEntity is not null)
                {
                    foodAmountEntity.FoodGuid = orderUpdateFoodModel.FoodGuid;
                    foodAmountEntity.OrderGuid = orderUpdateFoodModel.OrderGuid;
                    foodAmountEntity.Amount = orderUpdateFoodModel.Amount;
                    foodAmountEntity.Note = orderUpdateFoodModel.Note;
                }
            }
        }

        private void DeleteFoodAmounts(IEnumerable<FoodAmountEntity> foodAmountsToDelete)
        {
            var toDelete = foodAmountsToDelete.ToList();
            for (int i = 0; i < toDelete.Count; i++)
            {
                var foodAmountEntity = toDelete.ElementAt(i);
                foodAmounts.Remove(foodAmountEntity);
            }
        }

        private void InsertFoodAmounts(OrderEntity existingEntity,
            IEnumerable<FoodAmountEntity> orderFoodModelsToInsert)
        {
            foreach (var foodModel in orderFoodModelsToInsert)
            {
                foodAmounts.Add(new FoodAmountEntity
                {
                    Id = foodModel.Id,
                    FoodGuid = foodModel.FoodGuid,
                    OrderGuid = foodModel.OrderGuid,
                    Amount = foodModel.Amount,
                    Note = foodModel.Note
                });
            }
        }

        private IList<FoodAmountEntity> GetFoodAmountsByOrderId(Guid orderId)
        {
            return foodAmounts.Where(foodAmountEntity => foodAmountEntity.OrderGuid == orderId).ToList();
        }

        private FoodAmountEntity? GetFoodAmountOrderIdAndFoodId(Guid orderId, Guid foodId)
        {
            return foodAmounts.SingleOrDefault(entity => entity.OrderGuid == orderId && entity.FoodGuid == foodId);
        }

        public void Remove(Guid id)
        {
            var foodAmountsToRemove = foodAmounts.Where(foodAmount => foodAmount.OrderGuid == id).ToList();

            for (var i = 0; i < foodAmountsToRemove.Count; i++)
            {
                var foodAmountToRemove = foodAmountsToRemove.ElementAt(i);
                foodAmounts.Remove(foodAmountToRemove);
            }

            var orderToRemove = orders.SingleOrDefault(orderEntity => orderEntity.Id == id);
            if (orderToRemove is not null)
            {
                orders.Remove(orderToRemove);
            }
        }

        public bool Exists(Guid id)
        {
            return orders.Any(order => order.Id == id);
        }
    }
}
