using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using DameChales.Common.Enums;
using DameChales.Common.Models;
using DameChales.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace DameChales.Web.App.Pages
{
    public partial class FoodEditPage
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        private RestaurantFacade RestaurantFacade { get; set; } = null!;

        [Inject]
        private FoodFacade FoodFacade { get; set; } = null!;

        private FoodDetailModel Data { get; set; } = GetNewFoodDetailModel();

        [Parameter]
        public Guid Id { get; init; }
        private ICollection<RestaurantListModel> Restaurants { get; set; } = new List<RestaurantListModel>();

        private RestaurantDetailModel RestaurantToFood { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            if (Id != Guid.Empty)
            {
                Data = await FoodFacade.GetByIdAsync(Id);
            }

            Restaurants = await RestaurantFacade.GetAllAsync();

            await base.OnInitializedAsync();
        }

        public async Task Save()
        {
            if (Data.Name != "")
            {
                await FoodFacade.SaveAsync(Data);
                NavigationManager.NavigateTo($"/foods");
            }

        }

        public async Task Delete()
        {
            if (Data.Name != "")
            {
                await FoodFacade.DeleteAsync(Id);
                NavigationManager.NavigateTo($"/foods");
            }
        }
        private static FoodDetailModel GetNewFoodDetailModel() 
            => new()
            {
                Id = Guid.NewGuid(),
                Name = String.Empty,
                Description = string.Empty,
                Price = 0,
                PhotoURL = string.Empty
            };
    }
}
