using System.Collections.Generic;
using System.Threading.Tasks;
using DameChales.Common.Models;
using DameChales.Web.BL.Facades;
using Microsoft.AspNetCore.Components;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DameChales.Web.App.Pages
{
    public partial class RestaurantsDetailPage
    {
        [Inject]
        private NavigationManager navigationManager { get; set; } = null!;
        [Inject]
        private FoodFacade foodFacade { get; set; } = null!;
        [Inject]
        private RestaurantFacade restaurantFacade { get; set; } = null!;
        private ICollection<FoodListModel> foods { get; set; } = new List<FoodListModel>();
        private RestaurantDetailModel? restaurantDetailModel { get; set; }

        [Parameter]
        public Guid Id { get; set; } = Guid.Empty;

        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                navigationManager.NavigateTo($"/restaurants");
            }
            restaurantDetailModel = await restaurantFacade.GetByIdAsync( Id );
            foods = await foodFacade.GetByRestaurantIdAsync( Id );

            await base.OnInitializedAsync();
        }

    }
}
