using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DameChales.API.DAL.Common.Entities;
using DameChales.API.DAL.Common.Repositories;
using DameChales.Common.Models;

namespace DameChales.API.BL.Facades
{
    public class RestaurantFacade : IRestaurantFacade
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IMapper mapper;
           
        public RestaurantFacade(
            IRestaurantRepository restaurantRepository,
            IMapper mapper)
        {
            this.restaurantRepository = restaurantRepository;
            this.mapper = mapper;
        }

        public List<RestaurantListModel> GetAll()
        {
            var restaurantEntities = restaurantRepository.GetAll();
            return mapper.Map<List<RestaurantListModel>>(restaurantEntities);
        }

        public List<RestaurantListModel> GetByFoodId(Guid id)
        {
            var restaurantEntities = restaurantRepository.GetByFoodId(id);
            return mapper.Map<List<RestaurantListModel>>(restaurantEntities);
        }
        public RestaurantDetailModel? GetByName(string name)
        {
            var restaurantEntity = restaurantRepository.GetByName(name);
            return mapper.Map<RestaurantDetailModel>(restaurantEntity);
        }
        public RestaurantDetailModel? GetByAddress(string address)
        {
            var restaurantEntity = restaurantRepository.GetByAddress(address);
            return mapper.Map<RestaurantDetailModel>(restaurantEntity);
        }

        public RestaurantDetailModel? GetById(Guid id)
        {
            var restaurantEntity = restaurantRepository.GetById(id);
            return mapper.Map<RestaurantDetailModel>(restaurantEntity);
        }

        public Guid CreateOrUpdate(RestaurantDetailModel restaurantModel)
        {
            return restaurantRepository.Exists(restaurantModel.Id)
                ? Update(restaurantModel)!.Value
                : Create(restaurantModel);
        }

        //todo odtialto pokracovat
        public Guid Create(RestaurantDetailModel restaurantModel)
        {
            MergeOrdersAndFoods(recipeModel);
            var restaurantEntity = mapper.Map<RestaurantEntity>(restaurantModel);
            return restaurantRepository.Insert(restaurantEntity);
        }

        public Guid? Update(RestaurantDetailModel restaurantModel)
        {
            MergeOrdersAndFoods(restaurantModel);

            var restaurantEntity = mapper.Map<RestaurantEntity>(restaurantModel);
            
            restaurantEntity.Orders = restaurantModel.Orders.Select(t =>
                new OrderEntity(t.Id, restaurantEntity.Id, t.DeliveryTime, t.Note, t.Status, t.FoodAmounts)).ToList();

            restaurantEntity.Foods = restaurantModel.Foods.Select(t =>
                new FoodEntity(t.Id, t.Name, t.PhotoURL, t.Description, t.Price, restaurantEntity.Id, t.alergens)).ToList();
           
            
            var result = restaurantRepository.Update(restaurantEntity);

            return result;
        }

        public void MergeOrdersAndFoods(RestaurantDetailModel restaurant)
        {
            // order
            var result = new List<OrderFoodAmountDetailModel>();
            var orderGroups = restaurant.Orders.GroupBy(t => $"{t.FoodAmounts.FoodEntity.Id}");

            foreach (var orderGroup in orderGroups)
            {
                var orderFirst = orderGroup.First();
                var totalAmount = orderGroup.Sum(t => t.FoodAmounts.Amount);
                var order = new RestaurantDetailOrderModel(orderFirst.Id, totalAmount, orderFirst.FoodAmountEntity);

                result.Add(order);
            }

            restaurant.Orders = result;

            // food
            var result1 = new List<RestaurantDetailFoodModel>();
            var foodGroups = restaurant.Foods.GroupBy(t => $"{t.Id}");

            foreach (var foodGroup in foodGroups)
            {
                var foodFirst = foodGroup.First();
                var food = new RestaurantDetailFoodModel(foodFirst.Id);

                result.Add(food);
            }

            restaurant.Foods = result1;
        }

        public void Delete(Guid id)
        {
            restaurantRepository.Remove(id);
        }
    }
}
