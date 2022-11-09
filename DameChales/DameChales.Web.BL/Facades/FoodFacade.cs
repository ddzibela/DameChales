using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DameChales.Common.Models;
using DameChales.Web.BL.Options;
using DameChales.Web.DAL.Repositories;
using Microsoft.Extensions.Options;

namespace DameChales.Web.BL.Facades
{
    public class FoodFacade : FacadeBase<FoodDetailModel, FoodListModel>
    {
        private readonly IFoodApiClient apiClient;

        public FoodFacade(
            IFoodApiClient apiClient,
            FoodRepository foodRepository,
            IMapper mapper,
            IOptions<LocalDbOptions> localDbOptions)
            : base(foodRepository, mapper, localDbOptions)
        {
            this.apiClient = apiClient;
        }

        public override async Task<List<FoodListModel>> GetAllAsync()
        {
            var foodsAll = await base.GetAllAsync();

            var foodsFromApi = await apiClient.IngredientGetAsync(apiVersion, culture);
            foodsAll.AddRange(foodsFromApi);

            return foodsAll;
        }

        public override async Task<FoodDetailModel> GetByIdAsync(Guid id)
        {
            return await apiClient.FoodGetAsync(id, apiVersion, culture);
        }

        protected override async Task<Guid> SaveToApiAsync(FoodDetailModel data)
        {
            return await apiClient.UpsertAsync(apiVersion, culture, data);
        }

        public override async Task DeleteAsync(Guid id)
        {
            await apiClient.FoodDeleteAsync(id, apiVersion, culture);
        }
    }
}
