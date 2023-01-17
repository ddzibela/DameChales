﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DameChales.Common.Models;
using DameChales.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace DameChales.Web.App.Pages
{
    public partial class RestaurantsEditPage
    {
        [Inject]
        private NavigationManager navigationManager { get; set; } = null!;
        
        [Inject]
        private RestaurantFacade? RestaurantFacade { get; set; } = null;

        private RestaurantDetailModel? restaurantDetailModel { get; set; } = GetNewRestaurantDetailModel();

        [Parameter]
        public Guid Id { get; init; }

        protected override async Task OnInitializedAsync()
        {
            if (Id != Guid.Empty)
            {
                restaurantDetailModel = await RestaurantFacade.GetByIdAsync(Id);
            }
            await base.OnInitializedAsync();
        }

        public async Task Save()
        {
            if (restaurantDetailModel.Name != "" && restaurantDetailModel.Description != "" && restaurantDetailModel.Address != "")
            {
                await RestaurantFacade.SaveAsync(restaurantDetailModel);
                navigationManager.NavigateTo($"/restaurants");
            }
        }

        public async Task Delete()
        {
            if (restaurantDetailModel.Name != "" && restaurantDetailModel.Description != "" && restaurantDetailModel.Address != "")
            {
                await RestaurantFacade.DeleteAsync(Id);
                navigationManager.NavigateTo($"/restaurants");
            }
        }

        private static RestaurantDetailModel GetNewRestaurantDetailModel()
            => new()
            {
                Id = Guid.NewGuid(),
                Address = string.Empty,
                Description= string.Empty,
                GPSCoordinates = string.Empty,
                Name= string.Empty,
                PhotoURL= string.Empty,
                Foods = new List<FoodListModel>(),
                Orders = new List<OrderListModel>()
            };
    }
}
