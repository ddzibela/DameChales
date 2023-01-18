using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using DameChales.Common.Models;
using DameChales.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace DameChales.Web.App.Pages
{
    public partial class OrderEditPage
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;
        
        [Inject]
        private OrderFacade? OrderFacade { get; set; } = null;

        [Inject]
        private FoodFacade FoodFacade { get; set; } = null!;

        private OrderDetailModel? OrderDetailModel { get; set; } = GetNewOrderDetailModel();

        private ICollection<FoodListModel> Foods { get; set; } = new List<FoodListModel>();

        [Parameter]
        public Guid Id { get; init; }

        protected override async Task OnInitializedAsync()
        {
            if (Id != Guid.Empty)
            {
                OrderDetailModel = await OrderFacade.GetByIdAsync(Id);
            }
            Foods = await FoodFacade.GetByRestaurantIdAsync(OrderDetailModel.RestaurantGuid);
            await base.OnInitializedAsync();
        }

        public async Task Save()
        {
            if (OrderDetailModel.Name != "" && OrderDetailModel.Address != "") { 
                await OrderFacade.SaveAsync(OrderDetailModel);
                NavigationManager.NavigateTo($"/restaurants/orders/{OrderDetailModel.RestaurantGuid}");
            }
        }

        public async Task Delete()
        {
            if (OrderDetailModel.Name != "" && OrderDetailModel.Address != "")
            {
                await OrderFacade.DeleteAsync(Id);
                NavigationManager.NavigateTo($"/restaurants/orders/{OrderDetailModel.RestaurantGuid}");
            }
        }

        private static OrderDetailModel GetNewOrderDetailModel()
            => new()
            {
                Id = Guid.NewGuid(),
                RestaurantGuid = Guid.Empty,
                Name = string.Empty,
                Note = string.Empty,
                DeliveryTime = DateTime.MinValue,
                Status = Common.Enums.OrderStatus.Accepted,
                Address = string.Empty,
                FoodAmounts = new List<OrderFoodAmountDetailModel>()
            };
    }
}
