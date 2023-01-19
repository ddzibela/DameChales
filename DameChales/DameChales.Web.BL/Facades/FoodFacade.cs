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
            return (List<FoodListModel>)await apiClient.FoodGetAsync();

        }

        public override async Task<FoodDetailModel> GetByIdAsync(Guid id)
        {
            return await apiClient.FoodGetAsync(id);
        }

        public async Task<List<FoodListModel>> GetByNameAsync(string name)
        {
            return (List<FoodListModel>)await apiClient.NameGetAsync(name);
        }

        public async Task<List<FoodListModel>> GetByNameAsync(Guid id, string name)
        {

            return (List<FoodListModel>)await apiClient.NameGetAsync(id, name);

        }

        public async Task<List<FoodListModel>> GetByDescAsync(string desc)
        {
            return (List<FoodListModel>)await apiClient.DescGetAsync(desc);
        }

        public async Task<List<FoodListModel>> GetByDescAsync(Guid id, string desc)
        {
            return (List<FoodListModel>)await apiClient.NameGetAsync(id, desc);
        }

        public async Task<List<FoodListModel>> GetByRestaurantIdAsync(Guid id)
        {

            return (List<FoodListModel>)await apiClient.RestaurantAsync(id);
        }

        public async Task<List<FoodListModel>> GetWithoutAlergensAsync(Guid id, string alergensstr)
        {
            return (List<FoodListModel>)await apiClient.NoalergensGetAsync(id, alergensstr);
        }

        public async Task<List<FoodListModel>> GetWithoutAlergensAsync(string alergensstr)
        {
            return (List<FoodListModel>) await apiClient.NoalergensGetAsync(alergensstr);
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
