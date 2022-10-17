using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DameChales.API.DAL.Common.Entities;
using DameChales.API.DAL.Common.Repositories;

namespace DameChales.API.DAL.Memory.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly IList<RestaurantEntity> restaurants;
        private readonly IList<OrderEntity> orders;
        private readonly IList<FoodEntity> foods;
        private readonly IList<FoodAmountEntity> foodAmounts;

        public RestaurantRepository(Storage storage)
        {
            restaurants = storage.Restaurants;
            orders = storage.Orders;
            foods = storage.Foods;
            foodAmounts = storage.FoodAmounts;
        }

        public IList<RestaurantEntity> GetAll()
        {
            return restaurants;
        }

        public RestaurantEntity? GetById(Guid id)
        {
            return restaurants.SingleOrDefault(e => e.Id == id);
        }

        public Guid Insert(RestaurantEntity restaurant)
        {
            restaurants.Add(restaurant);
            return restaurant.Id;
        }

        public Guid? Update(RestaurantEntity restaurant)
        {

        }

        public void Remove(Guid id)
        {
            var ordersToRemove = orders.Where(e => e.RestaurantGuid == id).ToList();

            foreach (var order in ordersToRemove)
            {
                var foodAmountsToDelete = foodAmounts.Where(e => e.OrderGuid == order.Id).ToList();
                foreach (var amount in foodAmountsToDelete)
                {
                    foodAmounts.Remove(amount);
                }
                orders.Remove(order);
            }

            var restaurantToRemove = restaurants.Single(e => e.Id == id);
            restaurants.Remove(restaurantToRemove);

        }

        public bool Exists(Guid id)
        {
            return restaurants.Any(e => e.Id == id);
        }
    }

}
