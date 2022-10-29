using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using DameChales.API.DAL.Common.Entities;
using DameChales.API.DAL.Common.Entities.Interfaces;
using DameChales.API.DAL.Common.Repositories;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace DameChales.API.DAL.Memory.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly IList<RestaurantEntity> restaurants;
        private readonly IList<OrderEntity> orders;
        private readonly IList<FoodEntity> foods;
        private readonly IList<FoodAmountEntity> foodAmounts;
        private readonly IMapper mapper;

        public RestaurantRepository(Storage storage, IMapper mapper)
        {
            restaurants = storage.Restaurants;
            orders = storage.Orders;
            foods = storage.Foods;
            foodAmounts = storage.FoodAmounts;
            this.mapper = mapper;
        }

        public IList<RestaurantEntity> GetAll()
        {
            return restaurants;
        }

        public RestaurantEntity? GetById(Guid id)
        {
            return restaurants.SingleOrDefault(e => e.Id == id);
        }

        public RestaurantEntity? GetByFoodId(Guid id)
        {
            return restaurants.Where(e => e.Foods.Any(f => f.Id == id)).SingleOrDefault();
        }

        public IList<RestaurantEntity> GetByName(string name)
        {
            var nameRegex = new Regex(name);
            return restaurants.Where(e => nameRegex.IsMatch(e.Name)).ToList();
        }

        public IList<RestaurantEntity> GetByAddress(string address)
        {
            var addressRegex = new Regex(address);
            return restaurants.Where(e => addressRegex.IsMatch(e.Address)).ToList();
        }

        public Guid Insert(RestaurantEntity restaurant)
        {
            restaurants.Add(restaurant);
            return restaurant.Id;
        }

        public Guid? Update(RestaurantEntity restaurant)
        {
            var restaurantExisting = foods.SingleOrDefault(e => e.Id == restaurant.Id);
            if (restaurantExisting != null)
            {
                mapper.Map(restaurant, restaurantExisting);
            }

            return restaurantExisting?.Id;
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

            var foodsToRemove = foods.Where(e => e.RestaurantGuid == id).ToList();

            foreach(var food in foodsToRemove)
            {
                foods.Remove(food);
            }

            var restaurantToRemove = restaurants.Single(e => e.Id == id);
            restaurants.Remove(restaurantToRemove);

        }

        public double GetEarnings(Guid id)
        {
            double earnings = 0;
            if (!Exists(id)) return double.NaN;
            var ordersToCount = orders.Where(e => e.RestaurantGuid == id);
            foreach(var order in ordersToCount)
            {
               foreach(var amount in order.FoodAmounts)
                {
                    earnings += (amount.FoodEntity.Price * amount.Amount);
                } 
            }
            return earnings;
        }

        public bool Exists(Guid id)
        {
            return restaurants.Any(e => e.Id == id);
        }
    }

}
