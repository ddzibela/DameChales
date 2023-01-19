using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using DameChales.Common.Enums;
using DameChales.Common.Models;
using DameChales.Web.BL.Options;
using DameChales.Web.DAL.Repositories;
using Microsoft.Extensions.Options;

namespace DameChales.Web.BL.Facades
{
    public class FoodFacade : FacadeBase<FoodDetailModel, FoodListModel>
    {
        private readonly IFoodClient apiClient;

        public FoodFacade(
            IFoodClient apiClient,
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

            var foodsFromApi = await apiClient.FoodGetAsync();
            foodsAll.AddRange(foodsFromApi);

            return foodsAll;
        }

        public override async Task<FoodDetailModel> GetByIdAsync(Guid id)
        {
            return await apiClient.FoodGetAsync(id);
        }

        public async Task<List<FoodListModel>> GetByNameAsync(string name)
        {
            var foodsAll = await base.GetAllAsync();

            var foodsFromApi = await apiClient.NameGetAsync(name);
            foodsAll.AddRange(foodsFromApi);

            return foodsAll;
        }

        public async Task<List<FoodListModel>> GetByRestaurantIdAsync(Guid id)
        {
            var foodsAll = await base.GetAllAsync();

            var foodsFromApi = await apiClient.RestaurantAsync(id);
            foodsAll.AddRange(foodsFromApi);

            return foodsAll;
        }

        public async Task<List<FoodListModel>> GetWithoutAlergensAsync(Guid id, string alergensstr)
        {
            var foodsAll = await base.GetAllAsync();

            var foodsFromApi = await apiClient.NoalergensGetAsync(id, alergensstr);
            foodsAll.AddRange(foodsFromApi);

            return foodsAll;
        }

        protected override async Task<Guid> SaveToApiAsync(FoodDetailModel data)
        {
            return await apiClient.UpsertAsync(data);
        }

        public override async Task DeleteAsync(Guid id)
        {
            await apiClient.FoodDeleteAsync(id);
        }
    }
}
