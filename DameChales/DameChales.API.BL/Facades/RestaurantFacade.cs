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
        public List<RestaurantListModel>? GetByName(string name)
        {
            var restaurantEntity = restaurantRepository.GetByName(name);
            return mapper.Map<List<RestaurantListModel>>(restaurantEntity);
        }
        public List<RestaurantListModel>? GetByAddress(string address)
        {
            var restaurantEntity = restaurantRepository.GetByAddress(address);
            return mapper.Map<List<RestaurantListModel>>(restaurantEntity);
        }

        public RestaurantDetailModel? GetById(Guid id)
        {
            var restaurantEntity = restaurantRepository.GetById(id);
            return mapper.Map<RestaurantDetailModel>(restaurantEntity);
        }

        public double GetEarnings(Guid id)
        {
            return restaurantRepository.GetEarnings(id);
        }

        public Guid CreateOrUpdate(RestaurantDetailModel restaurantModel)
        {
            return restaurantRepository.Exists(restaurantModel.Id)
                ? Update(restaurantModel)!.Value
                : Create(restaurantModel);
        }

        public Guid Create(RestaurantDetailModel restaurantModel)
        {
            var restaurantEntity = mapper.Map<RestaurantEntity>(restaurantModel);
            return restaurantRepository.Insert(restaurantEntity);
        }

        public Guid? Update(RestaurantDetailModel restaurantModel)
        {
            var restaurantEntity = mapper.Map<RestaurantEntity>(restaurantModel);
            var result = restaurantRepository.Update(restaurantEntity);

            return result;
        }

        public void Delete(Guid id)
        {
            restaurantRepository.Remove(id);
        }
    }
}
