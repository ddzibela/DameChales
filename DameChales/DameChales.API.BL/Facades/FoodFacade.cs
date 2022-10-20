using System;
using System.Collections.Generic;
using AutoMapper;
using DameChales.API.DAL.Common.Entities;
using DameChales.API.DAL.Common.Repositories;
using DameChales.Common.Enums;
using DameChales.Common.Models;

namespace DameChales.API.BL.Facades
{
    public class FoodFacade : IFoodFacade
    {
        private readonly IFoodRepository foodRepository;
        private readonly IMapper mapper;

        public FoodFacade(
            IFoodRepository foodRepository,
            IMapper mapper)
        {
            this.foodRepository = foodRepository;
            this.mapper = mapper;
        }

        public List<FoodListModel> GetAll()
        {
            return mapper.Map<List<FoodListModel>>(foodRepository.GetAll());
        }

        public List<FoodListModel> GetWithoutAlergens(HashSet<Alergens> alergens)
        {
            return mapper.Map<List<FoodListModel>>(foodRepository.GetWithoutAlergen(alergens));
        }

        public List<FoodListModel> GetByRestaurantId(Guid id)
        {
            return mapper.Map<List<FoodListModel>>(foodRepository.GetByRestaurantId(id));
        }

        public List<FoodListModel> GetByName(string name)
        {
            return mapper.Map<List<FoodListModel>>(foodRepository.GetByName(name));
        }

        public FoodDetailModel? GetById(Guid id)
        {
            var foodEntity = foodRepository.GetById(id);
            return mapper.Map<FoodDetailModel>(foodEntity);
        }


        public Guid CreateOrUpdate(FoodDetailModel foodModel)
        {
            return foodRepository.Exists(foodModel.Id)
                ? Update(foodModel)!.Value
                : Create(foodModel);
        }

        public Guid Create(FoodDetailModel foodModel)
        {
            var foodEntity = mapper.Map<FoodEntity>(foodModel);
            return foodRepository.Insert(foodEntity);
        }

        public Guid? Update(FoodDetailModel foodModel)
        {
            var foodEntity = mapper.Map<FoodEntity>(foodModel);
            return foodRepository.Update(foodEntity);
        }

        public void Delete(Guid id)
        {
            foodRepository.Remove(id);
        }
    }
}
