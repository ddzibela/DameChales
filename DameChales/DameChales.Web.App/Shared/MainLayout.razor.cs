using System.Threading.Tasks;
using DameChales.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace DameChales.Web.App
{
    public partial class MainLayout
    {
        [Inject]
        public RestaurantFacade RestaurantFacade { get; set; } = null!;

        [Inject]
        public OrderFacade OrderFacade { get; set; } = null!;

        [Inject]
        public FoodFacade FoodFacade { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;


        public async Task OnlineStatusChangedAsync(bool isOnline)
        {
            if (isOnline)
            {
                var dataChanged = false;
                dataChanged = dataChanged || await FoodFacade.SynchronizeLocalDataAsync();
                dataChanged = dataChanged || await OrderFacade.SynchronizeLocalDataAsync();
                dataChanged = dataChanged || await RestaurantFacade.SynchronizeLocalDataAsync();

                if (dataChanged)
                {
                    NavigationManager.NavigateTo(NavigationManager.Uri, true);
                }
            }
        }
    }
}