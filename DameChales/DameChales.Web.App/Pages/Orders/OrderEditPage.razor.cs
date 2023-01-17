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
        private NavigationManager navigationManager { get; set; } = null!;
        
        [Inject]
        private OrderFacade? OrderFacade { get; set; } = null;

        [Inject]
        private FoodFacade foodFacade { get; set; } = null!;

        private OrderDetailModel? orderDetailModel { get; set; } = GetNewOrderDetailModel();

        private ICollection<FoodListModel> foods { get; set; } = new List<FoodListModel>();

        [Parameter]
        public Guid Id { get; init; }

        protected override async Task OnInitializedAsync()
        {
            if (Id != Guid.Empty)
            {
                orderDetailModel = await OrderFacade.GetByIdAsync(Id);
            }
            foods = await foodFacade.GetByRestaurantIdAsync(orderDetailModel.RestaurantGuid);
            await base.OnInitializedAsync();
        }

        public async Task Save()
        {
            if (orderDetailModel.Name != "" && orderDetailModel.Address != "") { 
                await OrderFacade.SaveAsync(orderDetailModel);
                navigationManager.NavigateTo($"/restaurants/orders/{orderDetailModel.RestaurantGuid}");
            }
        }

        public async Task Delete()
        {
            if (orderDetailModel.Name != "" && orderDetailModel.Address != "")
            {
                await OrderFacade.DeleteAsync(Id);
                navigationManager.NavigateTo($"/restaurants/orders/{orderDetailModel.RestaurantGuid}");
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
