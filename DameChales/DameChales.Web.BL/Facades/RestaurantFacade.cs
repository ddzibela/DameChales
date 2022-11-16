using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using DameChales.Common.Models;
using DameChales.Web.BL.Options;
using DameChales.Web.DAL.Repositories;
using Microsoft.Extensions.Options;

namespace DameChales.Web.BL.Facades
{
    public class RestaurantFacade : FacadeBase<RestaurantDetailModel, RestaurantListModel>
    {
        private readonly IRestaurantClient apiClient;

        public RestaurantFacade(
            IRestaurantClient apiClient,
            RestaurantRepository restaurantRepository,
            IMapper mapper,
            IOptions<LocalDbOptions> localDbOptions)
            : base(restaurantRepository, mapper, localDbOptions)
        {
            this.apiClient = apiClient;
        }

        public override async Task<List<RestaurantListModel>> GetAllAsync()
        {
            var restaurantsAll = await base.GetAllAsync();

            var restaurantsFromApi = await apiClient.RestaurantGetAsync();
            foreach (var restaurantFromApi in restaurantsFromApi)
            {
                if (restaurantsAll.Any(r => r.Id == restaurantFromApi.Id) is false)
                {
                    restaurantsAll.Add(restaurantFromApi);
                }
            }

            return restaurantsAll;
        }

        public override async Task<RestaurantDetailModel> GetByIdAsync(Guid id)
        {
            return await apiClient.RestaurantGetAsync(id);
        }

        protected override async Task<Guid> SaveToApiAsync(RestaurantDetailModel data)
        {
            return await apiClient.UpsertAsync(data);
        }

        public override async Task DeleteAsync(Guid id)
        {
            await apiClient.RestaurantDeleteAsync(id);
        }

        public async Task<List<RestaurantListModel>> GetByNameAsync(string name)
        {
            var foodsAll = await base.GetAllAsync();

            var foodsFromApi = await apiClient.NameAsync(name);
            foodsAll.AddRange(foodsFromApi);

            return foodsAll;
        }

        public async Task<RestaurantDetailModel> GetByFoodIdAsync(Guid id)
        {
            return await apiClient.FoodAsync(id);
        }

        public async Task<List<RestaurantListModel>> GetByAddressAsync(string address)
        {
            var foodsAll = await base.GetAllAsync();

            var foodsFromApi = await apiClient.AddressAsync(address);
            foodsAll.AddRange(foodsFromApi);

            return foodsAll;
        }

        public async Task<double> GetEarningsAsync(Guid id)
        {
            return await apiClient.EarningsAsync(id);
        }
    }
}
