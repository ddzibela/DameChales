using System.Collections.Generic;
using System.Threading.Tasks;
using DameChales.Common.Models;
using DameChales.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace DameChales.Web.App.Pages
{
    public partial class OrderListPage
    {
        [Inject]
        private OrderFacade orderFacade { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; } = null!;
        [Parameter]
        public Guid Id { get; set; }

        public IList<OrderListModel> allOrders { get; set; } = new List<OrderListModel>();
        public IList<OrderListModel> acceptedOrders { get; set; } = new List<OrderListModel>();
        public IList<OrderListModel> preparingOrders { get; set; } = new List<OrderListModel>();
        public IList<OrderListModel> deliveringOrders { get; set; } = new List<OrderListModel>();
        public IList<OrderListModel> deliveredOrders { get; set; } = new List<OrderListModel>();
        
        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                navigationManager.NavigateTo("/restaurants");
            }
            allOrders = await orderFacade.GetByRestaurantIdAsync(Id);
            
            acceptedOrders = allOrders.Where(x => x.Status == Common.Enums.OrderStatus.Accepted).ToList();
            preparingOrders = allOrders.Where(x => x.Status == Common.Enums.OrderStatus.Preparing).ToList();
            deliveringOrders = allOrders.Where(x => x.Status == Common.Enums.OrderStatus.Delivering).ToList();
            deliveredOrders = allOrders.Where(x => x.Status == Common.Enums.OrderStatus.Delivered).ToList();
            
            await base.OnInitializedAsync();
        }

        public async Task UpdateOrderStatus(OrderListModel order)
        {
            var _order = await orderFacade.GetByIdAsync(order.Id);
            _order.Status = order.Status;
            await orderFacade.SaveAsync(_order);
        }
    }
}
