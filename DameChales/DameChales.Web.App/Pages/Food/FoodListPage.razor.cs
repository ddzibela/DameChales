using System.Collections.Generic;
using System.Threading.Tasks;
using DameChales.Common.Models;
using DameChales.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace DameChales.Web.App.Pages
{
    public partial class FoodListPage
    {
        [Inject]
        private FoodFacade FoodFacade { get; set; } = null!;
        private RestaurantFacade RestaurantFacade { get; set; } = null!;
        private ICollection<FoodListModel> Foods { get; set; } = new List<FoodListModel>();
        protected override async Task OnInitializedAsync()
        {
            Foods = await FoodFacade.GetAllAsync();

            await base.OnInitializedAsync();
        }
    }
}
