using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper.Execution;
using DameChales.Common.Enums;
using DameChales.Common.Models;
using DameChales.Web.BL.Facades;
using DameChales.Web.DAL.Repositories;
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

        private IList<OrderListModel> allOrders { get; set; } = new List<OrderListModel>();
        private IList<OrderListModel> acceptedOrders { get; set; } = new List<OrderListModel>();
        private IList<OrderListModel> preparingOrders { get; set; } = new List<OrderListModel>();
        private IList<OrderListModel> deliveringOrders { get; set; } = new List<OrderListModel>();
        private IList<OrderListModel> deliveredOrders { get; set; } = new List<OrderListModel>();

        private OrderDetailModel tmp { get; set; } = GetNewOrderDetailModel();
        private IList<OrderDetailModel> acceptedOrdersDetail { get; set; } = new List<OrderDetailModel>();
        private IList<OrderDetailModel> preparingOrdersDetail { get; set; } = new List<OrderDetailModel>();

        private IList<OrderDetailModel> deliveringOrdersDetail { get; set; } = new List<OrderDetailModel>();
        private IList<OrderDetailModel> deliveredOrdersDetail { get; set; } = new List<OrderDetailModel>();

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

            foreach (var order in acceptedOrders)
            {
                tmp = await orderFacade.GetByIdAsync(order.Id);
                acceptedOrdersDetail.Add(tmp);
            }

            foreach (var order in preparingOrders)
            {
                tmp = await orderFacade.GetByIdAsync(order.Id);
                preparingOrdersDetail.Add(tmp);
            }

            foreach (var order in deliveringOrders)
            {
                tmp = await orderFacade.GetByIdAsync(order.Id);
                deliveringOrdersDetail.Add(tmp);
            }

            foreach (var order in deliveredOrders)
            {
                tmp = await orderFacade.GetByIdAsync(order.Id);
                deliveredOrdersDetail.Add(tmp);
            }

            await base.OnInitializedAsync();
        }

        public async Task UpdateOrderStatus(OrderDetailModel order)
        {
            await orderFacade.SaveAsync(order);
            navigationManager.NavigateTo($"/restaurants/orders/{order.RestaurantGuid}", true);
        }

        private static OrderDetailModel GetNewOrderDetailModel()
            => new()
            {
                Id = Guid.NewGuid(),
                RestaurantGuid = Guid.NewGuid(),
                Name = string.Empty,
                Note = string.Empty,
                DeliveryTime = DateTime.MinValue,
                Status = Common.Enums.OrderStatus.Accepted
            };
    }
}
